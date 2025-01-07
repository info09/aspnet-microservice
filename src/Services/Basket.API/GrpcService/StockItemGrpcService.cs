using Grpc.Core;
using Inventory.Grpc.Client;
using Polly;
using Polly.Retry;
using ILogger = Serilog.ILogger;

namespace Basket.API.GrpcService
{
    public class StockItemGrpcService
    {
        private readonly StockProtoService.StockProtoServiceClient _stockProtoServiceClient;
        private readonly ILogger _logger;
        private readonly AsyncRetryPolicy<StockModel> _retryPolicy;

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockProtoServiceClient, ILogger logger)
        {
            _stockProtoServiceClient = stockProtoServiceClient;
            _retryPolicy = Policy<StockModel>.Handle<RpcException>().RetryAsync(3);
            _logger = logger;
        }

        public async Task<StockModel> GetStock(string itemNo)
        {
            try
            {
                _logger.Information("BEGIN: Calling gRPC service for stock item {ItemNo}", itemNo);
                var stockItemRequest = new GetStockRequest { ItemNo = itemNo };

                return await _retryPolicy.ExecuteAsync(async () =>
                {
                    var result = await _stockProtoServiceClient.GetStockAsync(stockItemRequest);
                    if(result != null)
                        _logger.Information("END: Calling gRPC service for stock item {ItemNo} - StockValue {StockValue}", itemNo, result);
                    return result!;
                });

                
            }
            catch (RpcException e)
            {
                _logger.Error(e, "Error calling gRPC service for stock item {ItemNo}", itemNo);
                return new StockModel() { Quantity = -1 };
            }
        }
    }
}
