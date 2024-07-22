using Aiglusoft.IAM.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Infrastructure.Services
{
  public class RootContext : IRootContext
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RootContext(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public List<Claim> GetUserClaims(string schema = "")
    {
      var httpContext = _httpContextAccessor.HttpContext;
      if (httpContext == null)
      {
        return new List<Claim>();
      }
      if (!string.IsNullOrEmpty(schema))
      {
        var authenticateResult = httpContext.AuthenticateAsync(schema).Result;
        if (!authenticateResult.Succeeded || !authenticateResult.Principal.Identity.IsAuthenticated)
        {
          return new List<Claim>();
        }

        return authenticateResult.Principal.Claims.ToList();
      }

      return _httpContextAccessor.HttpContext.User.Claims.ToList();
    }

    public string GetUserEmail(string schema = "")
    {
      var httpContext = _httpContextAccessor.HttpContext;
      if (httpContext == null)
      {
        return null;
      }
      if (!string.IsNullOrEmpty(schema))
      {
        var authenticateResult = httpContext.AuthenticateAsync(schema).Result;
        if (!authenticateResult.Succeeded || !authenticateResult.Principal.Identity.IsAuthenticated)
        {
          return null;
        }

        var emailClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Email);
        return emailClaim?.Value;
      }

      return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public async Task<string> GetUserIdAsync(string schema = "")
    {
      var httpContext = _httpContextAccessor.HttpContext;
      if (httpContext == null)
      {
        return null;
      }
      if (!string.IsNullOrEmpty(schema))
      {
        var authenticateResult = await httpContext.AuthenticateAsync(schema);
        if (!authenticateResult.Succeeded || !authenticateResult.Principal.Identity.IsAuthenticated)
        {
          return null;
        }

        var userIdClaim = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim?.Value;
      }

      return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    }
  }
}
