using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode;
using Aiglusoft.IAM.Infrastructure.Services;
using Aiglusoft.IAM.Server.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aiglusoft.IAM.Server.Controllers
{

    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthorizationController(ISender mediator)
        {
            _sender = mediator;
        }

        /// <summary>
        /// Action pour l'endpoint /connect/authorize.
        /// Gère la demande d'autorisation OAuth 2.0.
        /// </summary>
        /// <param name="cancellationToken">Le jeton d'annulation facultatif.</param>
        /// <returns>Une action de redirection vers l'URL de redirection.</returns>
        [HttpGet("~/connect/authorize")]
        public async Task<IActionResult> GetAuthorize(CancellationToken cancellationToken = default)
        {
            var request = HttpContext.Request.Query.ToAuthorizeRequest();

            if (request.ResponseType != "code")
            {
                return BadRequest("Invalid response_type");
            }

            var command = new GenerateAuthorizationCodeCommand(request.ClientId, request.RedirectUri, request.State);
            var authorizationCode = await _sender.Send(command, cancellationToken);

            var uriBuilder = new UriBuilder(request.RedirectUri);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            query["code"] = authorizationCode;
            query["state"] = request.State;
            uriBuilder.Query = query.ToString();
            var redirectUrl = uriBuilder.ToString();

            return Redirect(redirectUrl);
        }

        [HttpPost("~/connect/token")]
        public async Task<IActionResult> PostToken(CancellationToken cancellationToken = default)
        {
            var request = HttpContext.Request.Form.ToTokenRequest();

            var command = new ExchangeAuthorizationCodeCommand(request.Code, request.ClientId, request.ClientSecret, request.RedirectUri, request.GrantType);
            try
            {
                var tokenResponse = await _sender.Send(command, cancellationToken);
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest("Invalid authorization code");
            }
        }



    }

}