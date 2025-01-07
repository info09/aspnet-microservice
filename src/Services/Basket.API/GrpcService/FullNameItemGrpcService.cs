using Customer.Grpc.Client;
using Grpc.Core;
using Polly;
using Polly.Retry;
using ILogger = Serilog.ILogger;

namespace Basket.API.GrpcService
{
    public class FullNameItemGrpcService
    {
        private readonly FullNameProtoService.FullNameProtoServiceClient _fullNameServiceClient;
        private readonly ILogger _logger;
        private readonly AsyncRetryPolicy<FullNameResponse> _retryPolicy;

        public FullNameItemGrpcService(FullNameProtoService.FullNameProtoServiceClient fullNameServiceClient, ILogger logger)
        {
            _fullNameServiceClient = fullNameServiceClient;
            _logger = logger;
            _retryPolicy = Policy<FullNameResponse>.Handle<RpcException>().RetryAsync(3);
        }

        public async Task<FullNameResponse> GetFullName(string userName)
        {
            try
            {
                _logger.Information("BEGIN: Calling gRPC service for full name of user {UserName}", userName);

                return await _retryPolicy.ExecuteAsync(async () =>
                {
                    var fullNameRequest = new GetFullNameRequest { UserName = userName };
                    var result = await _fullNameServiceClient.GetFullNameAsync(fullNameRequest);
                    if(result != null)
                        _logger.Information("END: Calling gRPC service for full name of user {UserName}", userName);
                    return result;
                });


            }
            catch (RpcException e)
            {
                _logger.Error(e, "Error calling gRPC service for full name of user {UserName}", userName);
                return new FullNameResponse() { FullName = string.Empty };
            }
        }
    }
}
