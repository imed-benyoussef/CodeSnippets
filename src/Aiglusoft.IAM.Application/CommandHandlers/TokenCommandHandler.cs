using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Repositories;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Localization;
using Aiglusoft.IAM.Domain.Services;

namespace Aiglusoft.IAM.Application.CommandHandlers
{
  public class TokenCommandHandler : IRequestHandler<TokenCommand, TokenResponse>
    {
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IStringLocalizer<ErrorMessages> _localizerErrorMessages;

        public TokenCommandHandler(
            IAuthorizationCodeRepository authorizationCodeRepository,
            IClientRepository clientRepository,
            ITokenRepository tokenRepository,
            ITokenFactory tokenFactory,
            IJwtTokenService jwtTokenService,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _authorizationCodeRepository = authorizationCodeRepository;
            _clientRepository = clientRepository;
            _tokenRepository = tokenRepository;
            _tokenFactory = tokenFactory;
            _jwtTokenService = jwtTokenService;
            _localizerErrorMessages = localizer;
        }

        public async Task<TokenResponse> Handle(TokenCommand request, CancellationToken cancellationToken)
        {
            // Validate client
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client == null || client.ClientSecret != request.ClientSecret)
            {
                throw new Exceptions.UnauthorizedAccessException(_localizerErrorMessages, "InvalidClientCredentials");
            }

            // Validate the authorization code
            var authorizationCode = await _authorizationCodeRepository.GetByCodeAsync(request.Code);
            if (authorizationCode == null || authorizationCode.ClientId != request.ClientId || authorizationCode.IsExpired())
            {
                throw new Exceptions.UnauthorizedAccessException(_localizerErrorMessages, "InvalidAuthorizationCode");
            }

            var accessTokenExpiry = DateTime.UtcNow.AddHours(1);
            // Generate the id_token only if the scope includes "openid"
            string idToken = null;
            if (authorizationCode.Scopes.Split(' ').Contains("openid"))
            {
                var idTokenClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, authorizationCode.User.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim("nonce", authorizationCode.CodeChallenge),
                    new Claim("scope", authorizationCode.Scopes)
                };
                idToken = _jwtTokenService.GenerateIdToken(idTokenClaims, accessTokenExpiry);
            }

            // Generate the access token using id_token if it's available
            var accessTokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, authorizationCode.User.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var accessToken = idToken != null
                ? _jwtTokenService.GenerateAccessToken(accessTokenClaims.Concat(new[] { new Claim("id_token", idToken) }), accessTokenExpiry)
                : _jwtTokenService.GenerateAccessToken(accessTokenClaims, accessTokenExpiry);

            var accessTokenEntity = _tokenFactory.CreateAccessToken(client, authorizationCode.User, accessTokenExpiry, accessTokenClaims);
            var refreshTokenEntity = _tokenFactory.CreateRefreshToken(client, authorizationCode.User, DateTime.UtcNow.AddDays(30));


            // Persist the tokens
            await _tokenRepository.AddAsync(accessTokenEntity);
            await _tokenRepository.AddAsync(refreshTokenEntity);

            return new TokenResponse
            {
                AccessToken = accessTokenEntity.Value,
                IdToken = idToken,
                TokenType = "Bearer",
                ExpiresIn = 3600, // 1 hour
                Scope = authorizationCode.Scopes,
                RefreshToken = refreshTokenEntity.Value
            };
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
