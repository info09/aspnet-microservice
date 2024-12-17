using Customer.Grpc.Persistence;
using Customer.Grpc.Repositories.Interfaces;
using Infrastructure.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Customer.Grpc.Repositories
{
    public class CustomerRepository : RepositoryQueryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext) : base(dbContext)
        {
        }

        public async Task<string?> GetFullNameCustomer(string userName)
        {
            var customer = await FindByCondition(i => i.UserName.Equals(userName)).SingleOrDefaultAsync();
            if (customer != null)
            {
                return customer.FirstName + " " + customer.LastName;
            }
            return null;
        }
    }
}
