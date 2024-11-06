using Customer.API.Services.Interfaces;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        public Task<IResult> GetCustomerByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
