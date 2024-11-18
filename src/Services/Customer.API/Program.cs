using Common.Logging;
using Customer.API;
using Customer.API.Controllers;
using Customer.API.Extensions;
using Customer.API.Persistence;
using Infrastructure.Middlewares;
using Infrastructure.ScheduleJob;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    // Add services to the container.
    builder.Host.AddApplicationConfiguration();
    builder.Services.AddConfigurationSettings(builder.Configuration);

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

    builder.Services.ConfigureCustomerContext();
    builder.Services.AddInfrastructureServices();
    builder.Services.AddTeduHangfireService();

    var app = builder.Build();

    app.MapGet("/", () => $"Welcome to {builder.Environment.ApplicationName}");

    app.MapCustomersAPI();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorWrappingMiddleware>();

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseHangfireDashboard(builder.Configuration);

    app.MapControllers();

    app.SeedCustomerData().Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Customer API complete");
    Log.CloseAndFlush();
}

