using Aiglusoft.IAM.Domain.Entities;
using Aiglusoft.IAM.Domain.Repositories;
using System.Collections.Concurrent;

namespace Aiglusoft.IAM.Infrastructure.Repositories
{
    public class AuthorizationCodeRepository : IAuthorizationCodeRepository
    {
        private static readonly ConcurrentDictionary<string, AuthorizationCode> Store = new ConcurrentDictionary<string, AuthorizationCode>();

        public Task SaveAsync(AuthorizationCode authorizationCode)
        {
            Store[authorizationCode.Code] = authorizationCode;
            return Task.CompletedTask;
        }

        public Task<AuthorizationCode> GetAsync(string code)
        {
            Store.TryGetValue(code, out var authorizationCode);
            return Task.FromResult(authorizationCode);
        }
    }
}
