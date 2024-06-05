using Aiglusoft.IAM.Application.Extentions;
using Aiglusoft.IAM.Domain.Entities;
using Aiglusoft.IAM.Domain.Repositories;
using MediatR;
using System.Web;

namespace Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode
{
    public class GenerateAuthorizationCodeCommandHandler : IRequestHandler<GenerateAuthorizationCodeCommand, string>
    {
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;

        public GenerateAuthorizationCodeCommandHandler(IAuthorizationCodeRepository authorizationCodeRepository)
        {
            _authorizationCodeRepository = authorizationCodeRepository;
        }

        public async Task<string> Handle(GenerateAuthorizationCodeCommand request, CancellationToken cancellationToken)
        {
            // Create a new authorization code
            var authorizationCode = new AuthorizationCode(request.ClientId);

            // Save the authorization code to the repository
            await _authorizationCodeRepository.SaveAsync(authorizationCode, cancellationToken);

            // Build the redirect URL with the authorization code and state
            var uriBuilder = new UriBuilder(request.RedirectUri);
            uriBuilder.RemoveDefaultPort();
            uriBuilder.AddOrUpdateQuery("code", authorizationCode.Code);
            uriBuilder.AddOrUpdateQuery("state", request.State);

            if (request.ResponseType.Contains("id_token"))
            {
                var idToken = "mock_id_token"; // Generate or retrieve the actual ID token

                uriBuilder.AddOrUpdateQuery("id_token", idToken);
                uriBuilder.AddOrUpdateQuery("nonce", request.Nonce);
            }

            var redirectUrl = uriBuilder.ToString();

            return redirectUrl;
        }
    }
}
