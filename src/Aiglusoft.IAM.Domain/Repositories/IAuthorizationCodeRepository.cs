using Aiglusoft.IAM.Domain.Entities;

namespace Aiglusoft.IAM.Domain.Repositories
{
    public interface IAuthorizationCodeRepository
    {
        Task SaveAsync(AuthorizationCode authorizationCode, CancellationToken cancellationToken);
        Task<AuthorizationCode> GetAsync(string code);
    }
}
