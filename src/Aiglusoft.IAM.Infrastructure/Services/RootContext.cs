using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Aiglusoft.IAM.Domain
{
    public class RootContext : IRootContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RootContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
