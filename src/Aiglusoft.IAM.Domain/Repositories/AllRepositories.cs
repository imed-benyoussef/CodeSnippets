using Aiglusoft.IAM.Domain.Entities;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string userId);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }

    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(string clientId);
        Task AddAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(Client client);
    }

    public interface IAuthorizationCodeRepository
    {
        Task<AuthorizationCode> GetByCodeAsync(string code);
        Task AddAsync(AuthorizationCode authorizationCode);
    }

    public interface ITokenRepository
    {
        Task<Token> GetByValueAsync(string tokenValue);
        Task AddAsync(Token token);
        Task UpdateAsync(Token token);
    }
}
