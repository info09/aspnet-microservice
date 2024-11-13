using Customer.Grpc.Client;

namespace Basket.API.GrpcService
{
    public class FullNameItemGrpcService
    {
        private readonly FullNameProtoService.FullNameProtoServiceClient _fullNameServiceClient;

        public FullNameItemGrpcService(FullNameProtoService.FullNameProtoServiceClient fullNameServiceClient)
        {
            _fullNameServiceClient = fullNameServiceClient;
        }

        public async Task<FullNameResponse> GetFullName(string userName)
        {
            try
            {
                var fullNameRequest = new GetFullNameRequest { UserName = userName };
                return await _fullNameServiceClient.GetFullNameAsync(fullNameRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
