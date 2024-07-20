
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    
}
