using Aiglusoft.IAM.Domain.Enums;

namespace Aiglusoft.IAM.Domain.Services
{
    public interface IVerificationCodeService
    {
        string GenerateVerificationCode(int length);
        Task SendVerificationCodeAsync(string contact, string code, VerificationChannel channel);
    }


}
