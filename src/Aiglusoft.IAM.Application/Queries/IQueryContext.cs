using Aiglusoft.IAM.Domain.Model.AuthorizationAggregates;
using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Domain.Model.CodeValidators;
using Aiglusoft.IAM.Domain.Model.TokenAggregate;
using Aiglusoft.IAM.Domain.Model.UserAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.Queries
{
  public interface IQueryContext
  {
    IQueryable<Client> Clients { get; }
    IQueryable<User> Users { get; }
    IQueryable<UserClaim> UserClaims { get; }
    IQueryable<ClientRedirectUri> ClientRedirectUris { get; }
    IQueryable<ClientScope> ClientScopes { get; }
    IQueryable<ClientGrantType> ClientGrantTypes { get; }
    IQueryable<AuthorizationCode> AuthorizationCodes { get; }
    IQueryable<Token> Tokens { get; }
    IQueryable<CodeValidator> CodeValidators { get; }

    IQueryable<T> GetQueryable<T>() where T : class;
  }
}
