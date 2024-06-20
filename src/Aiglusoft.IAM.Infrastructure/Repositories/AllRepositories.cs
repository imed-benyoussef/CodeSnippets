using Aiglusoft.IAM.Domain.Entities;
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
            return await _context.Users
                                 .Include(u => u.Claims)
                                 .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
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
            return await _context.Clients
                                 .Include(c => c.RedirectUris)
                                 .Include(c => c.Scopes)
                                 .Include(c => c.GrantTypes)
                                 .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task AddAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Client client)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
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
            return await _context.AuthorizationCodes
                                 .Include(ac => ac.Client)
                                 .Include(ac => ac.User)
                                 .FirstOrDefaultAsync(ac => ac.Code == code);
        }

        public async Task AddAsync(AuthorizationCode authorizationCode)
        {
            await _context.AuthorizationCodes.AddAsync(authorizationCode);
            await _context.SaveChangesAsync();
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
            return await _context.Tokens
                                 .Include(t => t.Client)
                                 .Include(t => t.User)
                                 .FirstOrDefaultAsync(t => t.Value == tokenValue);
        }

        public async Task AddAsync(Token token)
        {
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Token token)
        {
            _context.Tokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}
