using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcService;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services.Interfaces;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly StockItemGrpcService _stockItemGrpcService;
        private readonly FullNameItemGrpcService _fullNameItemGrpcService;
        private readonly IEmailTemplateService _emailTemplateService;

        public BasketsController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, IMapper mapper, StockItemGrpcService stockItemGrpcService = null, FullNameItemGrpcService fullNameItemGrpcService = null, IEmailTemplateService emailTemplateService = null)
        {
            _basketRepository = basketRepository;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _stockItemGrpcService = stockItemGrpcService;
            _fullNameItemGrpcService = fullNameItemGrpcService;
            _emailTemplateService = emailTemplateService;
        }

        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetBasket([Required] string username)
        {
            var result = await _basketRepository.GetBasketByUserName(username);

            return Ok(result ?? new Cart(username));
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
        {
            // Communicate with Customer.Grpc and check quantity available
            var fullNamResponse = await _fullNameItemGrpcService.GetFullName(cart.UserName);
            cart.SetFullName(fullNamResponse.FullName);
            // Communicate with Inventory.Grpc and check quantity available
            foreach (var item in cart.Items)
            {
                var stock = await _stockItemGrpcService.GetStock(item.ItemNo);
                item.SetAvailableQuantity(stock.Quantity);
            }

            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            var result = await _basketRepository.UpdateBasket(cart, options);
            return Ok(result);
        }

        [HttpDelete("{username}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteBasket([Required] string username)
        {
            var result = await _basketRepository.DeleteBasketFromUserName(username);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetBasketByUserName(basketCheckout.UserName);
            if (basket == null) return NotFound();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            _publishEndpoint.Publish(eventMessage);

            await _basketRepository.DeleteBasketFromUserName(basketCheckout.UserName);

            return Accepted();
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public ContentResult SendEmail()
        {
            var emailTemplate = _emailTemplateService.GenerateReminderCheckoutOrderEmail("huytq");
            var result = new ContentResult()
            {
                Content = emailTemplate,
                ContentType = "text/html"
            };
            return result;
        }
    }
}
