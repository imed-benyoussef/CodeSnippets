
namespace Aiglusoft.IAM.Infrastructure.Services
{
    using Aiglusoft.IAM.Domain.Services;
    using Aiglusoft.IAM.Domain.Enums;

    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;

        public VerificationCodeService(ISmsSender smsSender, IEmailSender emailSender)
        {
            _smsSender = smsSender;
            _emailSender = emailSender;
        }

        public string GenerateVerificationCode(int length)
        {
            var random = new Random();
            var code = string.Empty;
            for (int i = 0; i < length; i++)
            {
                code += random.Next(0, 10).ToString();
            }
            return code;
        }

        public async Task SendVerificationCodeAsync(string contact, string code, VerificationChannel channel)
        {
            switch (channel)
            {
                case VerificationChannel.SMS:
                    await _smsSender.SendSmsAsync(contact, $"Your verification code is: {code}");
                    break;
                case VerificationChannel.Email:
                    await _emailSender.SendEmailAsync(contact, "Verification Code", $"Your verification code is: {code}");
                    break;
                default:
                    throw new ArgumentException("Invalid verification channel");
            }
        }

    }

    }
