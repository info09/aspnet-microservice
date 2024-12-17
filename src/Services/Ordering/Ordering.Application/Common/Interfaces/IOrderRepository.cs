using Contracts.Domains.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepositoryBaseAsync<Order, long>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);

        Task<Order?> GetOrderByDocumentNoAsync(string documentNo);
    }
}
