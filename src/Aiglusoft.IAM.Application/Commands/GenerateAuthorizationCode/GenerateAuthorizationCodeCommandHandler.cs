using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Application.Extentions;
using Aiglusoft.IAM.Domain.Entities;
using Aiglusoft.IAM.Domain.Repositories;
using FluentValidation;
using MediatR;
using System.Web;

namespace Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode
{
    public class GenerateAuthorizationCodeCommandHandler : IRequestHandler<GenerateAuthorizationCodeCommand, string>
    {
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;
        private readonly IEnumerable<IValidator<GenerateAuthorizationCodeCommand>> _validators;

        public GenerateAuthorizationCodeCommandHandler(IAuthorizationCodeRepository authorizationCodeRepository, IEnumerable<IValidator<GenerateAuthorizationCodeCommand>> validators)
        {
            _authorizationCodeRepository = authorizationCodeRepository;
            _validators = validators;
        }

        public async Task<string> Handle(GenerateAuthorizationCodeCommand request, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<GenerateAuthorizationCodeCommand>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    foreach (var failure in failures)
                    {
                        throw new OAuthException(failure.ErrorCode, failure.ErrorMessage);
                    }
                }
            }

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
