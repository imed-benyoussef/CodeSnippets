using System.Linq;
using System.Threading.Tasks;
using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Server.Extensions;
using Aiglusoft.IAM.Server.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Aiglusoft.IAM.Server.Controllers
{
    [ApiController]
    [Route("connect")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRootContext _rootContext;

        public AuthorizationController(IMediator mediator, IRootContext rootContext)
        {
            _mediator = mediator;
            _rootContext = rootContext;
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize()
        {
            // Check if the user is authenticated
            var userId = await _rootContext.GetUserIdAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge(new AuthenticationProperties { RedirectUri = $"{Request.Path}{Request.QueryString}" }, CookieAuthenticationDefaults.AuthenticationScheme);
            }

            var request = HttpContext.GetOidcServerRequest();

            var command = new AuthorizeCommand
            {
                ResponseType = request.ResponseType,
                ClientId = request.ClientId,
                RedirectUri = request.RedirectUri,
                State = request.State,
                Scope = request.Scope,
                CodeChallenge = request.CodeChallenge,
                CodeChallengeMethod = request.CodeChallengeMethod,
                Nonce = request.Nonce,
                Prompt = request.Prompt,
                MaxAge = request.MaxAge,
                Display = request.Display,
                AcrValues = request.AcrValues,
                IdTokenHint = request.IdTokenHint,
                LoginHint = request.LoginHint
            };

            if (!IsValidResponseType(command.ResponseType))
            {
                return BadRequest(new
                {
                    error = "unsupported_response_type",
                    error_description = "ResponseType must include 'code'."
                });
            }

            try
            {
                var redirectUriResult = await _mediator.Send(command);
                return Redirect(redirectUriResult);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.Errors.FirstOrDefault()?.ErrorCode ?? "invalid_request",
                    error_description = ex.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid request parameters."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                var errorParts = ex.Message.Split(": ");
                return Unauthorized(new
                {
                    error = errorParts[0],
                    error_description = errorParts.Length > 1 ? errorParts[1] : ex.Message
                });
            }
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token()
        {
            var request = HttpContext.GetOidcServerRequest();

            if (request == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "invalid_request",
                    ErrorDescription = "Request body is missing."
                });
            }

            var command = new TokenCommand
            {
                GrantType = request.GrantType,
                Code = request.Code,
                RedirectUri = request.RedirectUri,
                ClientId = request.ClientId,
                ClientSecret = request.ClientSecret
            };

            try
            {
                var tokenResponse = await _mediator.Send(command);
                var jsonResponse = new
                {
                    access_token = tokenResponse.AccessToken,
                    token_type = tokenResponse.TokenType,
                    expires_in = tokenResponse.ExpiresIn,
                    id_token = tokenResponse.IdToken,
                    refresh_token = tokenResponse.RefreshToken,
                    scope = tokenResponse.Scope
                };
                return Ok(jsonResponse);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.Errors.FirstOrDefault()?.ErrorCode ?? "invalid_request",
                    error_description = ex.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid request parameters."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                var errorParts = ex.Message.Split(": ");
                return Unauthorized(new
                {
                    error = errorParts[0],
                    error_description = errorParts.Length > 1 ? errorParts[1] : ex.Message
                });
            }
        }

        private bool IsValidResponseType(string responseType)
        {
            return responseType == "code" || responseType.Split(' ').Contains("code");
        }
    }
}
