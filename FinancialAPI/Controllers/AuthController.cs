using Microsoft.AspNetCore.Mvc;
using FinancialAPI.Services;
using FinancialAPI.Dtos;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var token = await _auth.LoginAsync(req);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> CurrentUser()
        {
            var user = await _auth.GetCurrentUserAsync(User);
            if (user == null) return Unauthorized();
            return Ok(user);
        }
    }
}
