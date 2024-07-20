using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Model;
using Aiglusoft.IAM.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using Aiglusoft.IAM.Domain.Exceptions;
using Aiglusoft.IAM.Domain.Model.AuthorizationAggregates;

namespace Aiglusoft.IAM.Application.CommandHandlers
{

  public class AuthorizeCommandHandler : IRequestHandler<AuthorizeCommand, string>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<AuthorizeCommand> _validator;
        private readonly ILogger<AuthorizeCommandHandler> _logger;
        private readonly IRootContext _rootContext;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public AuthorizeCommandHandler(
            IClientRepository clientRepository,
            IAuthorizationCodeRepository authorizationCodeRepository,
            IUserRepository userRepository,
            IValidator<AuthorizeCommand> validator,
            ILogger<AuthorizeCommandHandler> logger,
            IRootContext rootContext,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _clientRepository = clientRepository;
            _authorizationCodeRepository = authorizationCodeRepository;
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
            _rootContext = rootContext;
            _localizer = localizer;
        }

        public async Task<string> Handle(AuthorizeCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            //var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            //if (!validationResult.IsValid)
            //{
            //    var error = validationResult.Errors.FirstOrDefault();
            //    if (error != null)
            //    {
            //        throw new InvalidRequestException(error.ErrorMessage);
            //    }
            //    throw new InvalidRequestException("Invalid request parameters");
            //}

            // Validate client
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client == null)
            {
                _logger.LogError("Invalid client_id: {ClientId}", request.ClientId);
                throw new InvalidClientException(_localizer, "InvalidClient", request.ClientId);
            }

            // Validate redirect URI
            if (!client.RedirectUris.Any(r => r.RedirectUri == request.RedirectUri))
            {
                _logger.LogError("Invalid redirect_uri: {RedirectUri}", request.RedirectUri);
                throw new InvalidRedirectUriException(_localizer, "InvalidRedirectUri", "");
            }

            // Validate response type
            if (!IsValidResponseType(request.ResponseType))
            {
                _logger.LogError("Invalid response_type: {ResponseType}", request.ResponseType);
                throw new UnsupportedResponseTypeException(_localizer, "InvalidResponseType");
            }

            // Validate user
            var userId = await _rootContext.GetUserIdAsync();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Invalid user_id: {UserId}", userId);
                throw new Exceptions.UnauthorizedAccessException(_localizer, "UnauthorizedAccess");
            }

            // Generate authorization code
            var authorizationCode = new AuthorizationCode(
                client,
                user,
                request.RedirectUri,
                request.Scope,
                DateTime.UtcNow.AddMinutes(10),
                request.CodeChallenge,
                request.CodeChallengeMethod);

            await _authorizationCodeRepository.AddAsync(authorizationCode);

            // Build the redirect URI with authorization code
            var uriBuilder = new UriBuilder(request.RedirectUri);
            var query = $"code={authorizationCode.Code}&state={request.State}";
            if (!string.IsNullOrEmpty(request.Scope) && request.Scope.Contains("openid"))
            {
                query += $"&nonce={request.Nonce}";
            }
            uriBuilder.Query = query;

            _logger.LogInformation("Authorization code generated successfully for client_id: {ClientId}", request.ClientId);

            return uriBuilder.ToString();
        }

        private bool IsValidResponseType(string responseType)
        {
            var validResponseTypes = new[]
            {
                "code", "id_token", "token", "code id_token", "code token",
                "id_token token", "code id_token token"
            };
            return validResponseTypes.Contains(responseType);
        }
    }


    //public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
    //{
    //    private readonly IVerificationCodeService _verificationCodeService;
    //    private readonly IUserRepository _userRepository;

    //    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    //    {
    //       var user = await _userRepository.GetByIdAsync(request.UserId);
    //        // 2. Valider le code de vérification

    //        //user.VerifyEmail(request.VerificationCode);

    //        //bool isValid = await _verificationCodeService.ValidateVerificationCode(request.UserId, request.VerificationCode);
    //        //if (!isValid)
    //        //{
    //        //    throw new Exception("Invalid verification code");
    //        //}

    //        //// 3. Marquer l'adresse email de l'utilisateur comme vérifiée dans la base de données
    //        //await _verificationCodeService.MarkEmailAsVerified(request.UserId);

    //        return true;
    //    }
    //}

}
