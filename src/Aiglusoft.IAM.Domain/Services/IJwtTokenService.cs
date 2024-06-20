using System.Security.Claims;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expiry);
        string GenerateIdToken(IEnumerable<Claim> claims, DateTime expiry);
        ClaimsPrincipal ValidateToken(string token);
    }

}
