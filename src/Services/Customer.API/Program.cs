using Common.Logging;
using Customer.API;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Customer.API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Customer.API.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Start Basket API up");

try
{
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(options => options.UseNpgsql(connectionString));
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
       .AddScoped<ICustomerService, CustomerService>();

    var app = builder.Build();

    app.MapGet("/", () => $"Welcome to {builder.Environment.ApplicationName}");

    app.MapCustomersAPI();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

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

