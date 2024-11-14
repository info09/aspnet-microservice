using MediatR;
using Ordering.Application.Common.Models;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<ApiResult<OrderDto>>
    {
        public long Id { get; set; }

        public GetOrderByIdQuery(long id)
        {
            Id = id;
        }
    }
}
