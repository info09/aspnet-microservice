using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace Infrastructure.Policies
{
    public static class HttpClientRetryPolicy
    {
        public static IHttpClientBuilder UseImmediateHttpRetryPolicy(this IHttpClientBuilder builder)
        {
            return builder.AddPolicyHandler(ConfigureImmediateHttpRetry());
        }

        public static IHttpClientBuilder UseLinearHttpRetryPolicy(this IHttpClientBuilder builder)
        {
            return builder.AddPolicyHandler(ConfigureLinearHttpRetry());
        }

        public static IHttpClientBuilder UseExponentialHttpRetryPolicy(this IHttpClientBuilder builder)
        {
            return builder.AddPolicyHandler(ConfigureExponentialHttpRetry());
        }

        public static IHttpClientBuilder UseCircuitBreakerPolicy(this IHttpClientBuilder builder)
        {
            return builder.AddPolicyHandler(ConfigureCircuitBreakerPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30)
                );
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureImmediateHttpRetry()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .RetryAsync(3, (exception, retryCount, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception.Exception.Message}");
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureLinearHttpRetry()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(3), (exception, retryCount, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}");
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureExponentialHttpRetry()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, retryCount, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}");
                });
        }
    }
}
