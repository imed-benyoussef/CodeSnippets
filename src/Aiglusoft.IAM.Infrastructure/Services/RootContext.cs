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

    public List<Claim> GetUserClaims()
    {
      var claims = _httpContextAccessor.HttpContext.User.Claims;
      var transformedClaims = claims.Select(c => new Claim(TransformClaimType(c.Type), c.Value)).ToList();


      return transformedClaims;
    }


    public string FindFirstValue(string type)
    {
      var value = _httpContextAccessor.HttpContext.User.FindFirstValue(type);
      if (string.IsNullOrEmpty(value))
      {

        var claims = _httpContextAccessor.HttpContext.User.Claims;
        var transformedClaims = claims.Select(c => new Claim(TransformClaimType(c.Type), c.Value)).ToList();

        value = transformedClaims.FirstOrDefault(e=>e.Type == type)?.Value;
      }


      return value;
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


  }

}
