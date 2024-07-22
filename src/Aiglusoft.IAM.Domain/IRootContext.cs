using System.Security.Claims;

namespace Aiglusoft.IAM.Domain
{
    public interface IRootContext
    {
        Task<string> GetUserIdAsync(string schema = "");
        string GetUserEmail(string schema = "");
        List<Claim> GetUserClaims(string schema = "");
    }
}
