using Microsoft.AspNetCore.Mvc;
using SchedualingSystem.Interfaces;
using SchedualingSystem.Models.Authentication;

namespace SchedualingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model )
        {
            var result = await _identityService.LoginAsync(model);
            if( !string.IsNullOrEmpty(result?.Token) )
            {
                return Ok(result);
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var result = await _identityService.RegisterAsync(model);   
            return Ok(result);
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterViewModel model )
        {
            var result = await _identityService.RegisterAdminAsync(model);  
            return Ok(result);  
        }
    }
}
