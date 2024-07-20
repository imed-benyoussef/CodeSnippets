namespace Aiglusoft.IAM.Domain.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string to, string body);
    }

    
}
