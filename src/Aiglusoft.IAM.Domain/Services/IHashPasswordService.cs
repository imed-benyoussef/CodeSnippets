namespace Aiglusoft.IAM.Domain.Services
{
  public interface IHashPasswordService
  {
    string HashPassword(string password, string securityStamp);
    bool VerifyPassword(string password, string passwordHash, string securityStamp);
  }
}
