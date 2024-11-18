using Contracts.ScheduledJobs;
using Contracts.Services;
using Hangfire.API.Services;
using Hangfire.API.Services.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.ScheduleJob;
using Infrastructure.Services;
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

        public static IServiceCollection AddConfigurationServices(this IServiceCollection services)
        {
            return services.AddTransient<IScheduledJobService, ScheduledJobService>()
                .AddScoped<ISmtpEmailService, SmtpEmailService>()
                            .AddScoped<IBackgroundJobService, BackgroundJobService>()
                ;
        }
    }
}
