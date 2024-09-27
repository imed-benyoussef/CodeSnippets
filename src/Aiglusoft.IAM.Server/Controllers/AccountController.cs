using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Aiglusoft.IAM.Server.Models;
using Aiglusoft.IAM.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Asp.Versioning;
using Aiglusoft.IAM.Domain;
using Microsoft.AspNetCore.Http;

namespace Aiglusoft.IAM.Server.Controllers
{
  [Authorize]
  [ApiVersion(1.0)]
  [ApiController]
  [Route("api/v{version:apiVersion}/account")]
  public class AccountController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IRootContext _rootContext;

    public AccountController(IMediator mediator, IRootContext rootContext)
    {
      _mediator = mediator;
      _rootContext = rootContext;
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {

      var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      if (!authenticateResult.Succeeded || !authenticateResult.Principal.Identity.IsAuthenticated)
      {
        return Unauthorized();
      }

      return Ok(new { message = "You are logged in!" });
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

      var command = new LoginCommand
      {
        Username = loginModel.Username,
        Password = loginModel.Password
      };


      var user = await _mediator.Send(command);

      var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
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

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return Ok(new { Message = "Logout successful" });
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
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