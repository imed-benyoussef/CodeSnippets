using Aiglusoft.IAM.Domain.Entities;

namespace Aiglusoft.IAM.Domain
{
    public interface IHashPasswordService
    {
        string HashPassword(string password, string securityStamp);
        bool VerifyPassword(string password, string passwordHash, string securityStamp);
    }
}
