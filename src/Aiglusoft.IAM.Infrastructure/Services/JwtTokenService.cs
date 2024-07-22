using Aiglusoft.IAM.Domain.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aiglusoft.IAM.Infrastructure.Services
{

  public class JwtTokenService : IJwtTokenService
  {
    private readonly string _issuer;
    private readonly string _audience;
    private readonly RsaSecurityKey _signingKey;

    public JwtTokenService(IConfiguration configuration, ICertificateService certificateService)
    {
      _issuer = configuration["Jwt:Issuer"];
      _audience = configuration["Jwt:Audience"];

      var rsa = certificateService.GetRsaPrivateKey();
      _signingKey = new RsaSecurityKey(rsa)
      {
        KeyId = certificateService.GetKeyId()
      };
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expiry)
    {
      return GenerateToken(claims, expiry);
    }

    public string GenerateIdToken(IEnumerable<Claim> claims, DateTime expiry)
    {
      return GenerateToken(claims, expiry);
    }

    private string GenerateToken(IEnumerable<Claim> claims, DateTime expiry)
    {
      var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256);
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
      return tokenHandler.ValidateToken(token, GetValidationParameters(), out _);
    }

    private TokenValidationParameters GetValidationParameters()
    {
      return new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = _issuer,
        ValidateAudience = true,
        ValidAudience = _audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = _signingKey,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };
    }
  }
}
