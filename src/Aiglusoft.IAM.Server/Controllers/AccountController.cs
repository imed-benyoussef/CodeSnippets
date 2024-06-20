using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Aiglusoft.IAM.Server.Models;
using Aiglusoft.IAM.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Aiglusoft.IAM.Server.Controllers
{
    [ApiController]
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            if (command == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "invalid_request",
                    ErrorDescription = "Request body is missing."
                });
            }

            if (string.IsNullOrEmpty(command.Username) || string.IsNullOrEmpty(command.Email) || string.IsNullOrEmpty(command.Password))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "invalid_request",
                    ErrorDescription = "Username, email or password is missing."
                });
            }

            try
            {
                var userId = await _mediator.Send(command);
                return Ok(new { UserId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "registration_failed",
                    ErrorDescription = ex.Message
                });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "invalid_request",
                    ErrorDescription = "Request body is missing."
                });
            }

            if (string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "invalid_request",
                    ErrorDescription = "Username or password is missing."
                });
            }

            var command = new LoginCommand
            {
                Username = loginModel.Username,
                Password = loginModel.Password
            };

            try
            {
                var user = await _mediator.Send(command);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = loginModel.RememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return Ok(new { Message = "Login successful" });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ErrorResponse
                {
                    Error = "invalid_credentials",
                    ErrorDescription = "The username or password is incorrect."
                });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logout successful" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest(new { error = "invalid_request", error_description = "Email is required." });
            }

            var command = new ForgotPasswordCommand { Email = model.Email };
            await _mediator.Send(command);

            return Ok(new { message = "Password reset email sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest(new { error = "invalid_request", error_description = "Token and new password are required." });
            }

            var command = new ResetPasswordCommand
            {
                Token = model.Token,
                NewPassword = model.NewPassword
            };

            try
            {
                await _mediator.Send(command);
                return Ok(new { message = "Password has been reset." });
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new { error = "invalid_token", error_description = ex.Message });
            }
        }
    }

    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}