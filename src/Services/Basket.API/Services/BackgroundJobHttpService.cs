using Shared.Configurations;

namespace Basket.API.Services
{
    public class BackgroundJobHttpService
    {
        public HttpClient Client { get; set; }

        public string ScheduledJobUrl { get; set; }

        public BackgroundJobHttpService(HttpClient client, BackgroundJobSettings settings)
        {
            if (settings == null || string.IsNullOrEmpty(settings.ScheduledJobUrl) || string.IsNullOrEmpty(settings.HangfireUrl))
                throw new ArgumentNullException($"{nameof(BackgroundJobSettings)} is not configured properly");

            client.BaseAddress = new Uri(settings.HangfireUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            Client = client;
            ScheduledJobUrl = settings.ScheduledJobUrl;
        }
    }
}
