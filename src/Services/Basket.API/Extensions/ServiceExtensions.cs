using Basket.API.GrpcService;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.Grpc.Client;
using EventBus.Messages.IntegrationEvents.Interfaces;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Inventory.Grpc.Client;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;

namespace Basket.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);
            var cacheSettings = configuration.GetSection(nameof(CacheSettings))
                .Get<CacheSettings>();
            services.AddSingleton(cacheSettings);

            var backgroundJobSettings = configuration.GetSection(nameof(BackgroundJobSettings))
                .Get<BackgroundJobSettings>();
            services.AddSingleton(backgroundJobSettings);

            return services;
        }

        public static IServiceCollection ConfigureHttpClientService(this IServiceCollection services)
        {
            services.AddHttpClient<BackgroundJobHttpService>()
                .AddHttpMessageHandler<LoggingDelegatingHandler>();
            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services) =>
            services.AddScoped<IBasketRepository, BasketRepository>()
                    .AddTransient<ISerializeService, SerializeService>()
                    .AddTransient<IEmailTemplateService, BasketEmailTemplateService>()
                    .AddTransient<LoggingDelegatingHandler>()
            ;

        public static IServiceCollection ConfigureGrpcService(this IServiceCollection services)
        {
            var settings = services.GetOptions<GrpcSettings>(nameof(GrpcSettings));
            services.AddGrpcClient<StockProtoService.StockProtoServiceClient>(i => i.Address = new Uri(settings.StockUrl));
            services.AddScoped<StockItemGrpcService>();

            services.AddGrpcClient<FullNameProtoService.FullNameProtoServiceClient>(i => i.Address = new Uri(settings.GetFullNameUrl));
            services.AddScoped<FullNameItemGrpcService>();
            return services;
        }

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = services.GetOptions<CacheSettings>("CacheSettings");
            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new ArgumentNullException("Redis Connection string is not configured.");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = settings.ConnectionString;
            });
        }

        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
                throw new ArgumentNullException("EventBusSettings is not configured!");

            var mqConnection = new Uri(settings.HostAddress);

            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                });
                config.AddRequestClient<IBacketCheckoutEvent>();
            });
        }
    }
}
