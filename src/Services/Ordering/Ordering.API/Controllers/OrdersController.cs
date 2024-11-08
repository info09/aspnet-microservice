using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.Orders.Queries.GetOrders;
using System.Net;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
        }

        [HttpGet("{userName}", Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }


    }
}
