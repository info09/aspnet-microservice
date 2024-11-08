using MediatR;
using Ordering.Application.Common.Models;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<ApiResult<List<OrderDto>>>
    {
        public string Username { get; set; }

        public GetOrdersQuery(string username)
        {
            Username = username;
        }
    }
}
