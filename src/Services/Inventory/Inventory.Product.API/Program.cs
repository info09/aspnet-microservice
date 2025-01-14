using Common.Logging;
using HealthChecks.UI.Client;
using Infrastructure.Identity;
using Inventory.Product.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    // Add services to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.ConfigureMongoDbClient();
    builder.Services.AddInfrastructureServices();
    builder.Services.ConfigureHealthChecks();
    builder.Services.ConfigureSwagger();

    builder.Services.ConfigureAuthenticationHandler();
    builder.Services.ConfigureAuthorization();

    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.OAuthClientId("tedu-microservice_swagger");
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API V1");
            c.DisplayRequestDuration();
        });
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();


    app.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


    app.MapControllers();

    app.MigrateDatabase()
        .Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Inventory API complete");
    Log.CloseAndFlush();
}

