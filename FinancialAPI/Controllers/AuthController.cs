using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Realiza login e retorna um token JWT.
        /// </summary>
        /// <param name="req">Dados de autenticação do usuário.</param>
        /// <returns>Token JWT e tempo de expiração.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var token = await _auth.LoginAsync(req);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        /// <summary>
        /// Obtém o usuário autenticado atual.
        /// </summary>
        /// <returns>Informações do usuário autenticado.</returns>
        [Authorize]
        [HttpGet("current-user")]
        [ProducesResponseType(typeof(CurrentUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CurrentUser()
        {
            var user = await _auth.GetCurrentUserAsync(User);
            if (user == null) return Unauthorized();
            return Ok(user);
        }
    }
}
