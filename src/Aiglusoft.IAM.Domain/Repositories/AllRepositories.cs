using Aiglusoft.IAM.Domain.Model;
using Aiglusoft.IAM.Domain.Model.AuthorizationAggregates;
using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Domain.Model.TokenAggregate;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
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
        Task<bool> IsUsernameTakenAsync(string username);
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
