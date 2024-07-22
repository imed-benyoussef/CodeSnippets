using System.Security.Claims;

namespace Aiglusoft.IAM.Domain
{
    public interface IRootContext
    {
    string FindFirstValue(string type);

    //Task<string> GetUserIdAsync(string schema = "");
    //string GetUserEmail(string schema = "");
    List<Claim> GetUserClaims();
    }
}
