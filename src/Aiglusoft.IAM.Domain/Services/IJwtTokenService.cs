namespace Aiglusoft.IAM.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username);
    }
}
