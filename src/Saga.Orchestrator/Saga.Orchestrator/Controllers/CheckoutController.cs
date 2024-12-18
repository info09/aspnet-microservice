using Microsoft.AspNetCore.Mvc;
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

        public CheckoutController(ICheckoutSagaService checkoutSagaService)
        {
            _checkoutSagaService = checkoutSagaService;
        }

        [HttpPost]
        [Route("{userName}")]
        public async Task<IActionResult> CheckoutOrder([Required] string userName, [FromBody] BasketCheckoutDto basketCheckout)
        {
            var result = await _checkoutSagaService.CheckoutOrder(userName, basketCheckout);
            return Accepted(result);
        }
    }
}
