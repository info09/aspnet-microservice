using AutoMapper;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Shared.Dtos.Customer;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IResult> GetCustomerByUsernameAsync(string username)
        {
            var entity = await _customerRepository.GetCustomerByUserNameAsync(username);
            var result = _mapper.Map<CustomerDto>(entity);

            return Results.Ok(result);
        }
    }
}
