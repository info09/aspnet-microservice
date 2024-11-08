namespace Ordering.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
