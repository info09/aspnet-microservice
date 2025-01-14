using Infrastructure.Extensions;
using Inventory.Product.API.Services;
using Inventory.Product.API.Services.Interfaces;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Product.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            services.AddSingleton(databaseSettings);

            var apiConfiguration = configuration.GetSection(nameof(ApiConfiguration)).Get<ApiConfiguration>();
            services.AddSingleton(apiConfiguration);

            return services;
        }

        private static string getMongoConnectionString(this IServiceCollection services)
        {
            var settings = services.GetOptions<MongoDbSettings>(nameof(MongoDbSettings));
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentNullException("DatabaseSettings is not configured");

            var databaseName = settings.DatabaseName;
            var mongodbConnectionString = settings.ConnectionString + "/" + databaseName +
                                          "?authSource=admin";
            return mongodbConnectionString;
        }

        public static void ConfigureMongoDbClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(new MongoClient(getMongoConnectionString(services)))
                .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
        }

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

            services.AddScoped<IInventoryService, InventoryService>();
        }

        public static void ConfigureHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddMongoDb(getMongoConnectionString(services), name: "MongoDb Health", failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded);
        }


        public static void ConfigureSwagger(this IServiceCollection services)
        {
            var configuration = services.GetOptions<ApiConfiguration>("ApiConfiguration");
            if (configuration == null || string.IsNullOrEmpty(configuration.IssuerUri) ||
                string.IsNullOrEmpty(configuration.ApiName)) throw new Exception("ApiConfiguration is not configured!");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Inventory API V1",
                        Version = configuration.ApiVersion
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{configuration.IdentityServerBaseUrl}/connect/authorize"),
                            Scopes = new Dictionary<string, string>
                        {
                            { "tedu-microservice_api.read", "Read Access to TeduMicroservice API" },
                            { "tedu-microservice_api.write", "Write Access to TeduMicroservice API" }
                        }
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer"
                        },
                        new List<string>
                        {
                            "tedu-microservice_api.read",
                            "tedu-microservice_api.write"
                        }
                    }
                });
            });

        }
    }
}
