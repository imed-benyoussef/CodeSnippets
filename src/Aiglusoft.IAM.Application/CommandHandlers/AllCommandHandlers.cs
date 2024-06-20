using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.DTOs;
using Aiglusoft.IAM.Application.Exceptions;
using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Entities;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        public AuthorizeCommandHandler(
            IClientRepository clientRepository,
            IAuthorizationCodeRepository authorizationCodeRepository,
            IUserRepository userRepository,
            IValidator<AuthorizeCommand> validator,
            ILogger<AuthorizeCommandHandler> logger,
            IRootContext rootContext)
        {
            _clientRepository = clientRepository;
            _authorizationCodeRepository = authorizationCodeRepository;
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
            _rootContext = rootContext;
        }

        public async Task<string> Handle(AuthorizeCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var error = validationResult.Errors.FirstOrDefault();
                if (error != null)
                {
                    throw new InvalidRequestException(error.ErrorMessage);
                }
                throw new InvalidRequestException("Invalid request parameters");
            }

            // Validate client
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client == null)
            {
                _logger.LogError("Invalid client_id: {ClientId}", request.ClientId);
                throw new InvalidClientException("Invalid client_id");
            }

            // Validate redirect URI
            if (!client.RedirectUris.Any(r => r.RedirectUri == request.RedirectUri))
            {
                _logger.LogError("Invalid redirect_uri: {RedirectUri}", request.RedirectUri);
                throw new InvalidRedirectUriException("Invalid redirect_uri");
            }

            // Validate response type
            if (!IsValidResponseType(request.ResponseType))
            {
                _logger.LogError("Invalid response_type: {ResponseType}", request.ResponseType);
                throw new UnsupportedResponseTypeException("Invalid response_type");
            }

            // Validate user
            var userId = await _rootContext.GetUserIdAsync();
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Invalid user_id: {UserId}", userId);
                throw new UnauthorizedAccessException("Invalid user");
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

    public class TokenCommandHandler : IRequestHandler<TokenCommand, TokenResponse>
    {
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtTokenService _jwtTokenService;

        public TokenCommandHandler(
            IAuthorizationCodeRepository authorizationCodeRepository,
            IClientRepository clientRepository,
            ITokenRepository tokenRepository,
            ITokenFactory tokenFactory,
            IJwtTokenService jwtTokenService)
        {
            _authorizationCodeRepository = authorizationCodeRepository;
            _clientRepository = clientRepository;
            _tokenRepository = tokenRepository;
            _tokenFactory = tokenFactory;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<TokenResponse> Handle(TokenCommand request, CancellationToken cancellationToken)
        {
            // Validate client
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client == null || client.ClientSecret != request.ClientSecret)
            {
                throw new UnauthorizedAccessException("Invalid client credentials.");
            }

            // Validate the authorization code
            var authorizationCode = await _authorizationCodeRepository.GetByCodeAsync(request.Code);
            if (authorizationCode == null || authorizationCode.ClientId != request.ClientId || authorizationCode.IsExpired())
            {
                throw new UnauthorizedAccessException("Invalid authorization code.");
            }

            var accessTokenExpiry = DateTime.UtcNow.AddHours(1);
            // Generate the id_token only if the scope includes "openid"
            string idToken = null;
            if (authorizationCode.Scopes.Split(' ').Contains("openid"))
            {
                var idTokenClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, authorizationCode.User.UserId),
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
                new Claim(JwtRegisteredClaimNames.Sub, authorizationCode.User.UserId),
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

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if the username is already in use
            var existingUserByUsername = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUserByUsername != null)
            {
                throw new UserAlreadyExistsException("Username is already in use.");
            }

            // Check if the email is already in use
            var existingUserByEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                throw new UserAlreadyExistsException("Email is already in use.");
            }

            // Create a new user
            var user = _userFactory.CreateUser(request.Username, request.Email, request.Password);
            await _userRepository.AddAsync(user);

            return user.UserId;
        }
    }


    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hashPasswordService;

        public LoginCommandHandler(IUserRepository userRepository, IHashPasswordService hashPasswordService)
        {
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
        }

        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null || !_hashPasswordService.VerifyPassword(request.Password, user.PasswordHash, user.SecurityStamp))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email
            };
        }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IEmailService emailService,
            IConfiguration configuration,
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"User with email {request.Email} not found.");
                return Unit.Value; // Optionally, you might want to throw an exception here
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

            var token = _jwtTokenService.GenerateAccessToken(claims, DateTime.Now.AddMinutes(30));

            // Use the issuer URL from the configuration as the base URL for the reset link
            var issuer = _configuration["Jwt:Issuer"];
            var resetLink = $"{issuer}/reset-password?token={token}";

            // Send email
            await _emailService.SendAsync(user.Email, "Password Reset Request", $"Please reset your password using the following link: {resetLink}");

            return Unit.Value;
        }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hashPasswordService;
        private readonly IJwtTokenService _jwtTokenService;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            IHashPasswordService hashPasswordService,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _hashPasswordService = hashPasswordService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = _jwtTokenService.ValidateToken(request.Token);

                var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new SecurityTokenException("Invalid token.");
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new SecurityTokenException("Invalid token.");
                }

                var securityStamp = Guid.NewGuid().ToString();
                var passwordHash = _hashPasswordService.HashPassword(request.NewPassword, securityStamp);

                user.SetPassword(passwordHash, securityStamp);

                await _userRepository.UpdateAsync(user);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token.", ex);
            }
        }
    }
}
