using Customer.Grpc.Protos;
using Customer.Grpc.Repositories.Interfaces;
using Grpc.Core;
using ILogger = Serilog.ILogger;

namespace Customer.Grpc.Services
{
    public class CustomerService : FullNameProtoService.FullNameProtoServiceBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public override async Task<FullNameResponse> GetFullName(GetFullNameRequest request, ServerCallContext context)
        {
            _logger.Information($"BENGIN Get FullName of UserName: {request.UserName}");
            var fullName = await _customerRepository.GetFullNameCustomer(request.UserName);
            var result = new FullNameResponse() { FullName = fullName };
            _logger.Information($"END Get FullName of UserName: {request.UserName} - FullName: {result.FullName}");
            return result;
        }
    }
}
