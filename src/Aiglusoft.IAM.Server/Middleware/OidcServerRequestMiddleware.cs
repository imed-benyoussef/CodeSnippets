namespace Aiglusoft.IAM.Server.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using MediatR;
    using Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode;
    using Aiglusoft.IAM.Server.Extensions;
    using Aiglusoft.IAM.Application.Exceptions;

    public class OidcServerRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public OidcServerRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sender = context.RequestServices.GetRequiredService<ISender>();

            var cancellationToken = context.RequestAborted;

            if (context.Request.Path.Equals("/connect/authorize", StringComparison.OrdinalIgnoreCase))
            {
                var request = context.GetOidcServerRequest();

                try
                {

                    var command = new GenerateAuthorizationCodeCommand(
                     clientId:  request.ClientId,
                     redirectUri:  request.RedirectUri,
                     state:  request.State,
                     responseType:  request.ResponseType,
                     scope:  request.Scope,
                     nonce:  request.Nonce,
                     codeChallenge:  request.CodeChallenge,
                     codeChallengeMethod:  request.CodeChallengeMethod
                   );

                    var redirectUri = await sender.Send(command, cancellationToken);

                    context.Response.Redirect(redirectUri);

                }
                catch (OAuthException ex)
                {
                    var uriBuilder = new UriBuilder(request.RedirectUri);
                    var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["error"] = ex.Error;
                    query["error_description"] = ex.ErrorDescription;

                    if (!string.IsNullOrEmpty(request.State))
                    {
                        query["state"] = request.State;
                    }

                    uriBuilder.Query = query.ToString();
                    context.Response.Redirect(uriBuilder.ToString());
                }
            }
            else
            {
                await _next(context);
            }
        }
    }

}
