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
using Aiglusoft.IAM.Domain.Constants;
using Aiglusoft.IAM.Domain.Model.CodeValidators;
using Aiglusoft.IAM.Domain.Model;

namespace Aiglusoft.IAM.Application.UseCases.Registers.CheckUserEmail
{
    public class CheckUserEmailCommandHandler : IRequestHandler<CheckUserEmailCommand, TokenResponse>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IStringLocalizer<ErrorMessages> _localizerErrorMessages;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly ICodeValidatorRepository _codeValidatorRepository;

        public CheckUserEmailCommandHandler(
            IJwtTokenService jwtTokenService,
            ITokenRepository tokenRepository,
            IUserFactory userFactory,
            IUserRepository userRepository,
            IStringLocalizer<ErrorMessages> localizerErrorMessages,
            IVerificationCodeService verificationCodeService,
            IHashPasswordService hashPasswordService,
            ICodeValidatorRepository codeValidatorRepository)
        {
            _jwtTokenService = jwtTokenService;
            _tokenRepository = tokenRepository;
            _userFactory = userFactory;
            _userRepository = userRepository;
            _localizerErrorMessages = localizerErrorMessages;
            _verificationCodeService = verificationCodeService;
            _hashPasswordService = hashPasswordService;
            _codeValidatorRepository = codeValidatorRepository;
        }

        public async Task<TokenResponse> Handle(CheckUserEmailCommand request, CancellationToken cancellationToken)
        {

            // Check if the email is already in use
            var existingUserByEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                throw new UserAlreadyExistsException(_localizerErrorMessages, "UserAlreadyExists", request.Email);
            }

            // Create a new user
            var username = await GenerateUniqueUsername(request.FirstName, request.Lastname);

            await _codeValidatorRepository.InvalidatePreviousCodesAsync(request.Email);

            var codeValidator = new CodeValidator(request.Email, VerificationType.Email, TimeSpan.FromMinutes(15));

            await _codeValidatorRepository.SaveAsync(codeValidator, cancellationToken);

            await _codeValidatorRepository.UnitOfWork.SaveChangesAsync();

            // Generate access token
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Sub, codeValidator.Id.ToString()),
                new Claim(JwtClaimTypes.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.Email, request.Email),
                new Claim(JwtClaimTypes.EmailVerified, bool.FalseString),
                new Claim(JwtClaimTypes.GivenName, request.FirstName),
                new Claim(JwtClaimTypes.FamilyName, request.Lastname),
                new Claim(JwtClaimTypes.Name, $"{request.FirstName} {request.Lastname}"),
                new Claim(JwtClaimTypes.UniqueName, username),
                new Claim(JwtClaimTypes.Gender, request.Gender),
                new Claim(JwtClaimTypes.Birthdate, request.Birthdate.ToString()),
                new Claim(JwtClaimTypes.Zoneinfo, request.Birthdate.ToString()
                )
            };
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


        private async Task<string> GenerateUniqueUsername(string firstName, string lastName)
        {
            return await UsernameGenerator.GenerateUsernameAsync(
                firstName: firstName,
                lastName: lastName,
                async uid => (await _userRepository.GetByUsernameAsync(uid)) != null);
        }


    }
}
