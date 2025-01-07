using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace Common.Logging
{
    public class LoggingDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingDelegatingHandler> _logger;
        public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
        {
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Sending request to {request.RequestUri} - Method {request.Method} - Version {request.Version}");
                var response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                    _logger.LogInformation($"Received successful response from {response.RequestMessage?.RequestUri} - Status code {response.StatusCode}");
                else
                    _logger.LogWarning($"Received unsuccessful response from {response.RequestMessage?.RequestUri} - Status code {response.StatusCode}");
                return response;
            }
            catch (HttpRequestException ex)
            when (ex.InnerException is SocketException { SocketErrorCode: SocketError.ConnectionRefused })
            {
                var hostWithPort = request.RequestUri.IsDefaultPort ? request.RequestUri?.DnsSafeHost : $"{request.RequestUri?.DnsSafeHost}:{request.RequestUri?.Port}";
                _logger.LogCritical(ex, $"Unable to connect to {hostWithPort}");
            }
            return new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                RequestMessage = request
            };
        }
    }
}
