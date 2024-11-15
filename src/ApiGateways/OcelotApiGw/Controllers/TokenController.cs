using Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Identity;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken()
        {
            var request = new TokenRequest();
            return Ok(_tokenService.GetToken(request));
        }
    }
}
