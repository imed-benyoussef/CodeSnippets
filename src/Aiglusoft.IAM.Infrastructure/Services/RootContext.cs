using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class RootContext : IRootContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RootContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Claim> GetUserClaims(string schema = "Cookies")
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            var transformedClaims = claims.Select(c => new Claim(TransformClaimType(c.Type), c.Value)).ToList();


            return transformedClaims;
        }

        private string TransformClaimType(string claimType)
        {
            return claimType switch
            {
                JwtClaimTypes.XmlSoapName => JwtClaimTypes.Name,
                JwtClaimTypes.XmlSoapGivenName => JwtClaimTypes.GivenName,
                JwtClaimTypes.XmlSoapSurname => JwtClaimTypes.FamilyName,
                JwtClaimTypes.XmlSoapEmail => JwtClaimTypes.Email,
                JwtClaimTypes.XmlSoapRole => JwtClaimTypes.Role,
                JwtClaimTypes.XmlSoapDateOfBirth => JwtClaimTypes.Birthdate,
                JwtClaimTypes.XmlSoapGender => JwtClaimTypes.Gender,
                JwtClaimTypes.XmlSoapNameIdentifier => JwtClaimTypes.UniqueName,

                _ => claimType
            };
        }

        public string GetUserEmail()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            return  email;
        }

        public async Task<string> GetUserIdAsync(string schema = "Cookies")
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            var authenticateResult = await httpContext.AuthenticateAsync(schema);
            if (!authenticateResult.Succeeded || !authenticateResult.Principal.Identity.IsAuthenticated)
            {
                return null;
            }

            var userIdClaim = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }
    }

}
