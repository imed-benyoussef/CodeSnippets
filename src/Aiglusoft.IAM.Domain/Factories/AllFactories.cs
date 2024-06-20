using Aiglusoft.IAM.Domain.Entities;
using System.Security.Claims;

namespace Aiglusoft.IAM.Domain.Factories
{
    public interface IUserFactory
    {
        User CreateUser(string username, string email, string password);
    }

    public interface ITokenFactory
    {
        Token CreateAccessToken(Client client, User user, DateTime expiry, IEnumerable<Claim> claims);
        Token CreateRefreshToken(Client client, User user, DateTime dateTime);
    }
}
