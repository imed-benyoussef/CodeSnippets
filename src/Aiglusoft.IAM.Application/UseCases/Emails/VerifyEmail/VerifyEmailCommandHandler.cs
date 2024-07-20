using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using MediatR;
using System.Security.Claims;
using Microsoft.Extensions.Localization;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Constants;
using Aiglusoft.IAM.Domain.Model.CodeValidators;
using System.IdentityModel.Tokens.Jwt;

namespace Aiglusoft.IAM.Application.UseCases.Emails.VerifyEmail
{
    internal class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, TokenResponse>
    {

        private readonly ICodeValidatorRepository _codeValidatorRepository;
        private readonly IRootContext _rootContext;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizerErrorMessages;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IHashPasswordService _hashPasswordService;

        public VerifyEmailCommandHandler(IRootContext rootContext,
                                         IJwtTokenService jwtTokenService,
                                         ITokenRepository tokenRepository,
                                         IUserRepository userRepository,
                                         IStringLocalizer<ErrorMessages> localizerErrorMessages,
                                         IVerificationCodeService verificationCodeService,
                                         IHashPasswordService hashPasswordService,
                                         ICodeValidatorRepository codeValidatorRepository)
        {
            _rootContext = rootContext;
            _jwtTokenService = jwtTokenService;
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _localizerErrorMessages = localizerErrorMessages;
            _verificationCodeService = verificationCodeService;
            _hashPasswordService = hashPasswordService;
            _codeValidatorRepository = codeValidatorRepository;
        }

        public async Task<TokenResponse> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var existingUserByEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                throw new UserAlreadyExistsException(_localizerErrorMessages, "UserAlreadyExists", request.Email);
            }


            var claims = _rootContext.GetUserClaims();
            var sub = claims.FirstOrDefault(e => e.Type == JwtClaimTypes.Sub)?.Value;

            if (string.IsNullOrEmpty(sub) || !Guid.TryParse(sub, out var userId))
            {
                throw new Exceptions.UnauthorizedAccessException(_localizerErrorMessages, "Invalid user ID in claims.");
            }

            var codeValidator = await _codeValidatorRepository.GetAsync(userId);
            if (codeValidator == null || !codeValidator.IsValid(code: request.Code, target: request.Email))
            {
                throw new InvalidVerificationCodeException(_localizerErrorMessages, "InvalidVerificationCode", request.Email);
            }

            // Mark the code as used
            codeValidator?.MarkAsUsed();

            await _codeValidatorRepository.SaveAsync(codeValidator, cancellationToken);

            var c = claims.FirstOrDefault(e => e.Type == JwtClaimTypes.EmailVerified);
            claims.Remove(c);
            claims.Add(
                new Claim(JwtClaimTypes.EmailVerified, bool.TrueString));


            var expires_in = 18 * 60 * 1000;
            var expired_at = DateTime.Now.AddSeconds(expires_in);
            var accessTokenString = _jwtTokenService.GenerateAccessToken(claims, expired_at);
            var idTokenString = _jwtTokenService.GenerateAccessToken(claims, expired_at);


            return new TokenResponse
            {
                AccessToken = accessTokenString,
                TokenType = "Bearer",
                ExpiresIn = expires_in, // 30 minutes
                IdToken = idTokenString

            };
        }
    }
}