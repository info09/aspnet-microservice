using AutoMapper;
using EventBus.Messages.IntegrationEvents.Events;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Features.Orders.Common;
using Ordering.Domain.Entities;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Command.CreateOrder
{
    public class CreateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<long>>, IMapFrom<Order>, IMapFrom<BasketCheckoutEvent>
    {
        public string UserName { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrderCommand, Order>();
            profile.CreateMap<BasketCheckoutEvent, CreateOrderCommand>();
        }
    }
}
