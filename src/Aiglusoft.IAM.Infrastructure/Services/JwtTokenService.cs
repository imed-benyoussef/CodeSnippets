using Aiglusoft.IAM.Domain.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly ICertificateService _certificateService;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenService(IConfiguration configuration, ICertificateService certificateService)
        {
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _certificateService = certificateService;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expiry)
        {
            var rsa = _certificateService.GetRsaPrivateKey();
            var key = new RsaSecurityKey(rsa)
            {
                KeyId = _certificateService.GetKeyId()
            };
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateIdToken(IEnumerable<Claim> claims, DateTime expiry)
        {
            var rsa = _certificateService.GetRsaPrivateKey();
            var key = new RsaSecurityKey(rsa)
            {
                KeyId = _certificateService.GetKeyId()
            };
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.ValidateToken(token, GetValidationParameters(), out SecurityToken validatedToken);

        }
        public TokenValidationParameters GetValidationParameters()
        {
            var rsa = _certificateService.GetRsaPrivateKey();
            var key = new RsaSecurityKey(rsa)
            {
                KeyId = _certificateService.GetKeyId()
            };

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
