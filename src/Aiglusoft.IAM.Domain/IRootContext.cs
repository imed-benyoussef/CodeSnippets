namespace Aiglusoft.IAM.Domain
{
    public interface IRootContext
    {
        Task<string> GetUserIdAsync(string schema = "Cookies");
    }
}
