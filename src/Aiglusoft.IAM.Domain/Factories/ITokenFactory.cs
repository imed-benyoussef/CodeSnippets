using Aiglusoft.IAM.Domain.Model;
using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Domain.Model.TokenAggregate;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using System.Security.Claims;

namespace Aiglusoft.IAM.Domain.Factories
{
    public interface ITokenFactory
    {
        Token CreateAccessToken(Client client, User user, DateTime expiry, IEnumerable<Claim> claims);
        Token CreateRefreshToken(Client client, User user, DateTime dateTime);
    }
}
