using Contracts.Services;
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
        private readonly ISmtpEmailService _smtpEmailService;

        public OrdersController(IMediator mediator, ISmtpEmailService smtpEmailService)
        {
            _mediator = mediator;
            _smtpEmailService = smtpEmailService;
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

        [HttpGet]
        public async Task<IActionResult> SendEmail()
        {
            await _smtpEmailService.SendEmailAsync(new Shared.Services.Email.MailRequest()
            {
                Subject = "Demo",
                ToAddress = "huytq@ics-p.vn",
                Body ="Hello World"
            });
            return Ok();
        }
    }
}
