using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Command.CreateOrder;
using Ordering.Application.Features.Orders.Command.DeleteOrder;
using Ordering.Application.Features.Orders.Command.DeleteOrderByDocumentNo;
using Ordering.Application.Features.Orders.Command.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderById;
using Ordering.Application.Features.Orders.Queries.GetOrders;
using Shared.Dtos.Order;
using Shared.SeedWorks;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrdersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        private static class RouteNames
        {
            public const string GetOrdersByUserName = nameof(GetOrdersByUserName);
            public const string GetOrderById = nameof(GetOrderById);
            public const string CreateOrder = nameof(CreateOrder);
            public const string UpdateOrder = nameof(UpdateOrder);
            public const string DeleteOrder = nameof(DeleteOrder);
            public const string DeleteOrderByDocumentNo = nameof(DeleteOrderByDocumentNo);
        }

        [HttpGet("username/{userName}", Name = RouteNames.GetOrdersByUserName)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersByUserNameQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpGet("{id}", Name = RouteNames.GetOrderById)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<OrderDto>>> GetOrdersById(long id)
        {
            var query = new GetOrderByIdQuery(id);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpPost(Name = RouteNames.CreateOrder)]
        [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody] CreateOrderDto model)
        {
            var command = _mapper.Map<CreateOrderCommand>(model);
            var orders = await _mediator.Send(command);
            return Ok(orders);
        }

        [HttpPut("{id}", Name = RouteNames.UpdateOrder)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<OrderDto>>> UpdateOrder([Required] long id, [FromBody] UpdateOrderCommand command)
        {
            var orders = await _mediator.Send(command);
            return Ok(orders);
        }

        [HttpDelete("{id}", Name = RouteNames.DeleteOrder)]
        [ProducesResponseType(typeof(ApiResult<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<bool>>> DeleteOrder([Required] long id)
        {
            var command = new DeleteOrderCommand(id);
            var orders = await _mediator.Send(command);
            return Ok(orders);
        }

        [HttpDelete("document-no/{documentNo}", Name = RouteNames.DeleteOrderByDocumentNo)]
        [ProducesResponseType(typeof(ApiResult<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<bool>>> DeleteOrderByDocumentNo([Required] string documentNo)
        {
            var command = new DeleteOrderByDocumentNoCommand(documentNo);
            var orders = await _mediator.Send(command);
            return Ok(orders);
        }
    }
}
