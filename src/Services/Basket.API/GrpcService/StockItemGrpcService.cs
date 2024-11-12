using Inventory.Grpc.Client;

namespace Basket.API.GrpcService
{
    public class StockItemGrpcService
    {
        private readonly StockProtoService.StockProtoServiceClient _stockProtoServiceClient;

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockProtoServiceClient)
        {
            _stockProtoServiceClient = stockProtoServiceClient;
        }

        public async Task<StockModel> GetStock(string itemNo)
        {
            try
            {
                var stockItemRequest = new GetStockRequest { ItemNo = itemNo };
                return await _stockProtoServiceClient.GetStockAsync(stockItemRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
