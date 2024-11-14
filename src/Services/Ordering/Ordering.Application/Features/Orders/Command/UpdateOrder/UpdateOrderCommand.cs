using AutoMapper;
using Infrastructure.Extensions;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.Orders.Common;
using Ordering.Domain.Entities;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Command.UpdateOrder
{
    public class UpdateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<OrderDto>>, IMapFrom<Order>
    {
        public long Id { get; set; }
        public void SetId(long id)
        {
            Id = id;
        }

        public void MappingProfile(Profile profile)
        {
            profile.CreateMap<UpdateOrderCommand, Order>().ForMember(dest => dest.Status, opts => opts.Ignore()).IgnoreAllNonExisting();
        }
    }
}
