
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
