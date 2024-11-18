using Infrastructure.Configurations;
using Shared.Configurations;

namespace Hangfire.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var hangfireSettings = configuration.GetSection(nameof(HangfireSettings)).Get<HangfireSettings>();
            services.AddSingleton(hangfireSettings);

            var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting)).Get<SMTPEmailSetting>();
            services.AddSingleton(emailSettings);

            return services;
        }
    }
}
