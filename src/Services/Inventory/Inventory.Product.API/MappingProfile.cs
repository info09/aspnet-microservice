using AutoMapper;
using Inventory.Product.API.Entities;
using Shared.Dtos.Inventory;

namespace Inventory.Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InventoryEntry, InventoryEntryDto>().ReverseMap();
            CreateMap<PurchaseProductDto, InventoryEntry>();
        }
    }
}
