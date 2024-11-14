using MediatR;
using Ordering.Application.Common.Models;
using Shared.SeedWorks;

namespace Ordering.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersByUserNameQuery : IRequest<ApiResult<List<OrderDto>>>
    {
        public string Username { get; set; }

        public GetOrdersByUserNameQuery(string username)
        {
            Username = username;
        }
    }
}
