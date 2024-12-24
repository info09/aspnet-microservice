using Contracts.Sagas.OrderManager;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.OrderManager;
using Saga.Orchestrator.Services.Interfaces;
using Shared.Dtos.Basket;
using System.ComponentModel.DataAnnotations;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutSagaService _checkoutSagaService;
        private readonly ISagaOrderManager<BasketCheckoutDto, OrderResponse> _orderManager;

        public CheckoutController(ICheckoutSagaService checkoutSagaService, ISagaOrderManager<BasketCheckoutDto, OrderResponse> orderManager)
        {
            _checkoutSagaService = checkoutSagaService;
            _orderManager = orderManager;
        }

        [HttpPost]
        [Route("{userName}")]
        public OrderResponse CheckoutOrder([Required] string userName, [FromBody] BasketCheckoutDto model)
        {
            model.UserName = userName;
            var result = _orderManager.CreateOrder(model);
            return result;
        }
    }
}
