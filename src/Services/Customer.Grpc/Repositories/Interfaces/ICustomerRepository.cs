using Contracts.Domains.Interfaces;
using Customer.Grpc.Persistence;

namespace Customer.Grpc.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryQueryBase<Entities.Customer, int, CustomerContext>
    {
        Task<string> GetFullNameCustomer(string userName);
    }
}
