using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryQueryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext) : base(dbContext)
        {
        }

        public Task<Entities.Customer> GetCustomerByUserNameAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
