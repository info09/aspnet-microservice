using Saga.Orchestrator.HttpRepository;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services;
using Saga.Orchestrator.Services.Interfaces;

namespace Saga.Orchestrator.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICheckoutSagaService, CheckoutSagaService>();
            return services;
        }

        public static IServiceCollection ConfigureHttpRepository(this IServiceCollection services)
        {
            services.AddScoped<IOrderHttpRepository, OrderHttpRepository>()
                .AddScoped<IInventoryHttpRepository, InventoryHttpRepository>()
                .AddScoped<IBasketHttpRepository, BasketHttpRepository>();
            return services;
        }

        public static void ConfigureHttpClients(this IServiceCollection services)
        {
            ConfigureOrderHttpClient(services);
            ConfigureBasketHttpClient(services);
            ConfigureInventoryHttpClient(services);
        }

        private static void ConfigureOrderHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IOrderHttpRepository, OrderHttpRepository>("OrdersAPI", (sp, cl) =>
            {
                cl.BaseAddress = new Uri("http://localhost:5005/api/");
            });
            services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
                .CreateClient("OrdersAPI"));
        }

        private static void ConfigureBasketHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IBasketHttpRepository, BasketHttpRepository>("BasketsAPI", (sp, cl) =>
            {
                cl.BaseAddress = new Uri("http://localhost:5004/api/");
            });
            services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
                .CreateClient("BasketsAPI"));
        }

        private static void ConfigureInventoryHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IInventoryHttpRepository, InventoryHttpRepository>("InventoryAPI", (sp, cl) =>
            {
                cl.BaseAddress = new Uri("http://localhost:5006/api/");
            });
            services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
                .CreateClient("InventoryAPI"));
        }
    }
}
