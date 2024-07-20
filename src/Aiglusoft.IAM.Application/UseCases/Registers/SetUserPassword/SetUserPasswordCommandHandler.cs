using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Domain;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.JsonWebTokens;
using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Domain.Constants;
using System.Security.Claims;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Model.CodeValidators;

namespace Aiglusoft.IAM.Application.UseCases.Registers.SetUserPassword
{
  internal class SetUserPasswordCommandHandler : IRequestHandler<SetUserPasswordCommand, TokenResponse>
  {

    private readonly IRootContext _rootContext;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserFactory _userFactory;
    private readonly IStringLocalizer<ErrorMessages> _localizerErrorMessages;
    private readonly IVerificationCodeService _verificationCodeService;
    private readonly IHashPasswordService _hashPasswordService;
    private readonly IEncryptionService _encryptionService;


    public SetUserPasswordCommandHandler(IRootContext rootContext,
                                     IJwtTokenService jwtTokenService,
                                     ITokenRepository tokenRepository,
                                     IUserRepository userRepository,
                                     IStringLocalizer<ErrorMessages> localizerErrorMessages,
                                     IVerificationCodeService verificationCodeService,
                                     IHashPasswordService hashPasswordService,
                                     IUserFactory userFactory,
                                     IEncryptionService encryptionService)
    {
      _rootContext = rootContext;
      _jwtTokenService = jwtTokenService;
      _tokenRepository = tokenRepository;
      _userRepository = userRepository;
      _localizerErrorMessages = localizerErrorMessages;
      _verificationCodeService = verificationCodeService;
      _hashPasswordService = hashPasswordService;
      _userFactory = userFactory;
      _encryptionService = encryptionService;
    }

    public async Task<TokenResponse> Handle(SetUserPasswordCommand request, CancellationToken cancellationToken)
    {

      var email = _rootContext.GetUserEmail();

      var existingUserByEmail = await _userRepository.GetByEmailAsync(email);
      if (existingUserByEmail != null)
      {
        throw new UserAlreadyExistsException(_localizerErrorMessages, "UserAlreadyExists", email);
      }

      var claims = _rootContext.GetUserClaims();
      var sub = claims.FirstOrDefault(e => e.Type == JwtClaimTypes.Sub)?.Value;

      // Extraire les autres informations utilisateur à partir des claims
      var firstName = claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.GivenName)?.Value;
      var lastName = claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.FamilyName)?.Value;
      var birthdateClaim = claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Birthdate)?.Value;
      var gender = claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Gender)?.Value;


      var username = await UsernameGenerator.GenerateUsernameAsync(firstName, lastName, async username => await _userRepository.IsUsernameTakenAsync(username));

      if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(birthdateClaim) || string.IsNullOrEmpty(gender))
      {
        throw new InvalidOperationException("Missing required user information in claims.");
      }

      if (!DateOnly.TryParse(birthdateClaim, out var birthdate))
      {
        throw new InvalidOperationException("Invalid birthdate format in claims.");
      }

      var decryptPassword = request.Password;

      // Créer un utilisateur en utilisant l'usine d'utilisateurs
      var user = _userFactory.CreateUser(
          username: username,
          email: email,
          password: decryptPassword,
          firstName: firstName,
          lastName: lastName,
          birthdate: birthdate,
          gender: gender
      );

      await _userRepository.AddAsync(user);

      // Generate access token
      var _claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Sub, user.Id),
                new Claim(JwtClaimTypes.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.EmailVerified, bool.TrueString),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtClaimTypes.UniqueName, username),
                new Claim(JwtClaimTypes.Gender, user.Gender),
                new Claim(JwtClaimTypes.Birthdate, user.Birthdate.ToString())
            };


      var expires_in = 18 * 60 * 1000;
      var expired_at = DateTime.Now.AddSeconds(expires_in);
      var idTokenString = _jwtTokenService.GenerateAccessToken(claims, expired_at);
      var accessTokenString = _jwtTokenService.GenerateAccessToken(claims, expired_at);


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
