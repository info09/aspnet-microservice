using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;

namespace Infrastructure.Policies
{
    public static class HttpClientRetryPolicy
    {
        public static IHttpClientBuilder UseImmediateHttpRetryPolicy(this IHttpClientBuilder builder, int retryCount = 3) => builder.AddPolicyHandler(ConfigureImmediateHttpRetry(retryCount));


        public static IHttpClientBuilder UseLinearHttpRetryPolicy(this IHttpClientBuilder builder, int retryCount = 3, int fromSecond = 30) => builder.AddPolicyHandler(ConfigureLinearHttpRetry(retryCount, fromSecond));


        public static IHttpClientBuilder UseExponentialHttpRetryPolicy(this IHttpClientBuilder builder, int retryCount = 5) => builder.AddPolicyHandler(ConfigureExponentialHttpRetry(retryCount));

        public static IHttpClientBuilder UseCircuitBreakerPolicy(this IHttpClientBuilder builder, int eventsAllowedBeforeBreaking = 3, int fromSecond = 30) =>
            builder.AddPolicyHandler(ConfigureCircuitBreakerPolicy(eventsAllowedBeforeBreaking, fromSecond));


        public static IHttpClientBuilder ConfigureTimeoutPolicy(this IHttpClientBuilder builder, int seconds = 5) =>
            builder.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(seconds));

        private static IAsyncPolicy<HttpResponseMessage> ConfigureCircuitBreakerPolicy(int eventsAllowedBeforeBreaking, int fromSecond) => HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: eventsAllowedBeforeBreaking,
                    durationOfBreak: TimeSpan.FromSeconds(fromSecond)
                );

        private static IAsyncPolicy<HttpResponseMessage> ConfigureImmediateHttpRetry(int retryCount) => HttpPolicyExtensions.HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .RetryAsync(retryCount, (exception, retryCount, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception.Exception.Message}");
                });

        private static IAsyncPolicy<HttpResponseMessage> ConfigureLinearHttpRetry(int retryCount, int fromSecond) => HttpPolicyExtensions.HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(fromSecond), (exception, retryCount, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}");
                });

        private static IAsyncPolicy<HttpResponseMessage> ConfigureExponentialHttpRetry(int retryCount) =>
            HttpPolicyExtensions.HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, retryCount, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}");
                });
    }
}
