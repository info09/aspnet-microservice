using Hangfire.API.Extensions;
using Infrastructure.ScheduleJob;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    builder.Host.AddApplicationConfiguration();
    // Add services to the container.

    builder.Services.AddConfigurationSettings(builder.Configuration);

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTeduHangfireService();
    builder.Services.AddConfigurationServices();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseHangfireDashboard(builder.Configuration);

    app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Hangfire API failed to start");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}


