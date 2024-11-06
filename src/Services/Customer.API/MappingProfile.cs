using AutoMapper;
using Shared.Dtos.Customer;

namespace Customer.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Customer, CustomerDto>();
        }
    }
}
