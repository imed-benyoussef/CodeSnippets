using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Aiglusoft.IAM.Domain.Factories;
using Aiglusoft.IAM.Domain.Model;
using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Domain.Model.TokenAggregate;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using Aiglusoft.IAM.Domain.Services;

namespace Aiglusoft.IAM.Infrastructure.Factories
{
    public class TokenFactory : ITokenFactory
    {
        private readonly IJwtTokenService _jwtTokenService;

        public TokenFactory(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public Token CreateAccessToken(Client client, User user, DateTime expiry, IEnumerable<Claim> claims)
        {
            var tokenValue = _jwtTokenService.GenerateAccessToken(claims, expiry);
            return new Domain.Model.TokenAggregate.Token(client, user, "access", expiry, tokenValue);
        }

        public Token CreateRefreshToken(Client client, User user, DateTime expiry)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var tokenValue = _jwtTokenService.GenerateAccessToken(claims, expiry);
            return new Token(client, user, "refresh", expiry, tokenValue);
        }
    }
}
