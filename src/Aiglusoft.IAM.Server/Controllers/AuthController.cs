using Aiglusoft.IAM.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aiglusoft.IAM.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginModel login)
        {
            // Valider les informations d'identification de l'utilisateur ici (cet exemple est simplifié)
            if (login.Username == "testuser" && login.Password == "password")
            {
                var token = _jwtTokenService.GenerateToken(login.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    }

}