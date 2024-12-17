using Contracts.Domains.Interfaces;
using Inventory.Grpc.Entities;

namespace Inventory.Grpc.Repositories.Interfaces
{
    public interface IInventoryRepository : IMongoDbRepositoryBase<InventoryEntry>
    {
        int GetStockQuantity(string itemNo);
    }
}
