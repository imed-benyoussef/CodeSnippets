using System.Security.Claims;

namespace Aiglusoft.IAM.Domain
{
    public interface IRootContext
    {
        Task<string> GetUserIdAsync(string schema = "Cookies");
        string GetUserEmail();
        List<Claim> GetUserClaims(string schema = "Cookies");
    }
}
