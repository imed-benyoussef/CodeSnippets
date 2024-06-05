using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode;
using Aiglusoft.IAM.Infrastructure.Services;
using Aiglusoft.IAM.Server.Extensions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

        ///// <summary>
        ///// Action pour l'endpoint /connect/authorize.
        ///// Gère la demande d'autorisation OAuth 2.0.
        ///// </summary>
        ///// <param name="cancellationToken">Le jeton d'annulation facultatif.</param>
        ///// <returns>Une action de redirection vers l'URL de redirection.</returns>
        //[HttpGet("~/connect/authorize")]
        //public async Task<IActionResult> GetAuthorize(CancellationToken cancellationToken = default)
        //{
        //    var request = HttpContext.GetOidcServerRequest();

        //    if (request.IsAuthorizationCodeRequest())
        //    {
        //        var command = new GenerateAuthorizationCodeCommand(request.ClientId, request.RedirectUri, request.State);
        //        var authorizationCode = await _sender.Send(command, cancellationToken);

        //        var uriBuilder = new UriBuilder(request.RedirectUri);
        //        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        //        query["code"] = authorizationCode;
        //        query["state"] = request.State;
        //        uriBuilder.Query = query.ToString();
        //        var redirectUrl = uriBuilder.ToString();

        //        return Redirect(redirectUrl);
        //    }

        //    throw new NotImplementedException("Le type de flux spécifié n'est pas implémenté.");

        //}

        [HttpPost("~/connect/token")]
        public async Task<IActionResult> PostToken(CancellationToken cancellationToken = default)
        {
            var request = HttpContext.GetOidcServerRequest();

            var command = new ExchangeAuthorizationCodeCommand(request.Code, request.ClientId, request.ClientSecret, request.RedirectUri, request.GrantType);

            var tokenResponse = await _sender.Send(command, cancellationToken);
            return Ok(tokenResponse);
        }
    }

}