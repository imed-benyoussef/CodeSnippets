using Aiglusoft.IAM.Domain.Model;
using Aiglusoft.IAM.Domain.Model.AuthorizationAggregates;
using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Domain.Model.TokenAggregate;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using Aiglusoft.IAM.Domain.Repositories;
using Aiglusoft.IAM.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;


namespace Aiglusoft.IAM.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _context.Set<User > ()
                                 .Include(u => u.Claims)
                                 .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Set<User > ().SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Set<User > ().SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Set<User > ().AddAsync(user);
            await _context.SaveEntitiesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Set<User > ().Update(user);
            await _context.SaveEntitiesAsync();
        }

        public Task<bool> IsUsernameTakenAsync(string username)
        {
          return _context.Set<User > ().AnyAsync(e=>e.Username.ToLower() == username.ToLower());
        }
    }

    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetByIdAsync(string clientId)
        {
            return await _context.Set<Client> ()
                                 .Include(c => c.RedirectUris)
                                 .Include(c => c.Scopes)
                                 .Include(c => c.GrantTypes)
                                 .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task AddAsync(Client client)
        {
            await _context.Set<Client> ().AddAsync(client);
            await _context.SaveEntitiesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            _context.Set<Client> ().Update(client);
            await _context.SaveEntitiesAsync();
        }

        public async Task DeleteAsync(Client client)
        {
            _context.Set<Client> ().Remove(client);
            await _context.SaveEntitiesAsync();
        }
    }

    public class AuthorizationCodeRepository : IAuthorizationCodeRepository
    {
        private readonly AppDbContext _context;

        public AuthorizationCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthorizationCode> GetByCodeAsync(string code)
        {
            return await _context.Set<AuthorizationCode> ()
                                 .Include(ac => ac.Client)
                                 .Include(ac => ac.User)
                                 .FirstOrDefaultAsync(ac => ac.Code == code);
        }

        public async Task AddAsync(AuthorizationCode authorizationCode)
        {
            await _context.Set<AuthorizationCode> ().AddAsync(authorizationCode);
            await _context.SaveEntitiesAsync();
        }
    }

    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _context;

        public TokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Token> GetByValueAsync(string tokenValue)
        {
            return await _context.Set<Token> ()
                                 .Include(t => t.Client)
                                 .Include(t => t.User)
                                 .FirstOrDefaultAsync(t => t.Value == tokenValue);
        }

        public async Task AddAsync(Token token)
        {
            await _context.Set<Token> ().AddAsync(token);
            await _context.SaveEntitiesAsync();
        }

        public async Task UpdateAsync(Token token)
        {
            _context.Set<Token> ().Update(token);
            await _context.SaveEntitiesAsync();
        }
    }

}
