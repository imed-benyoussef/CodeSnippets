using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using static Aiglusoft.IAM.Application.Commands.ExchangeAuthorizationCodeCommand;

namespace Aiglusoft.IAM.Application.Commands.Handlers
{
    public class ExchangeAuthorizationCodeCommandHandler : IRequestHandler<ExchangeAuthorizationCodeCommand, TokenResponse>
    {
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public ExchangeAuthorizationCodeCommandHandler(
            IAuthorizationCodeRepository authorizationCodeRepository,
            IJwtTokenService jwtTokenService,
            IConfiguration configuration)
        {
            _authorizationCodeRepository = authorizationCodeRepository;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        public async Task<TokenResponse> Handle(ExchangeAuthorizationCodeCommand request, CancellationToken cancellationToken)
        {
            var authorizationCode = await _authorizationCodeRepository.GetAsync(request.Code);
            if (authorizationCode == null || authorizationCode.ClientId != request.ClientId || authorizationCode.IsExpired())
            {
                throw new UnauthorizedAccessException("Invalid authorization code");
            }

            var idToken = _jwtTokenService.GenerateToken(request.ClientId);
            var accessToken = _jwtTokenService.GenerateToken(request.ClientId);

            return new TokenResponse
            {
                IdToken = idToken,
                AccessToken = accessToken,
                TokenType = "Bearer",
                ExpiresIn = 3600
            };
        }
    }
}
