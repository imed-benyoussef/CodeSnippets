using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace Aiglusoft.IAM.Domain.Entities
{
    public interface IAggregateRoot { }

    public class User : IAggregateRoot
    {
        public string UserId { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string Email { get; private set; }
        public bool EmailVerified { get; private set; }
        public string SecurityStamp { get; private set; }

        private List<UserClaim> _claims;
        public IReadOnlyCollection<UserClaim> Claims => _claims.AsReadOnly();

        internal User()
        {
            
        }
        public User(string username, string email, string passwordHash, string securityStamp)
        {
            UserId = Guid.NewGuid().ToString();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            EmailVerified = false;
            _claims = new List<UserClaim>();
            SecurityStamp = securityStamp;
        }

        public void VerifyEmail()
        {
            EmailVerified = true;
        }

        public void AddClaim(string type, string value)
        {
            _claims.Add(new UserClaim(this, type, value));
        }
        public void UpdateSecurityStamp()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public void SetPassword(string passwordHash, string securityStamp)
        {
            PasswordHash = passwordHash;
            SecurityStamp = securityStamp;
        }
    }

    public class UserClaim
    {
        public string UserClaimId { get; private set; }
        public string UserId { get; private set; }
        public string Type { get; private set; }
        public string Value { get; private set; }

        public User User { get; private set; }

        internal UserClaim()
        {
            
        }
        public UserClaim(User user, string type, string value)
        {
            UserClaimId = Guid.NewGuid().ToString();
            User = user;
            UserId = user.UserId;
            Type = type;
            Value = value;
        }
    }

    public class Client : IAggregateRoot
    {
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string ClientName { get; private set; }

        private List<ClientRedirectUri> _redirectUris;
        public IReadOnlyCollection<ClientRedirectUri> RedirectUris => _redirectUris.AsReadOnly();

        private List<ClientScope> _scopes;
        public IReadOnlyCollection<ClientScope> Scopes => _scopes.AsReadOnly();

        private List<ClientGrantType> _grantTypes;
        public IReadOnlyCollection<ClientGrantType> GrantTypes => _grantTypes.AsReadOnly();

        internal Client()
        {
            
        }
        public Client(string clientId, string clientSecret, string clientName)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            ClientName = clientName;
            _redirectUris = new List<ClientRedirectUri>();
            _scopes = new List<ClientScope>();
            _grantTypes = new List<ClientGrantType>();
        }

        public void AddRedirectUri(string redirectUri)
        {
            _redirectUris.Add(new ClientRedirectUri(this, redirectUri));
        }

        public void AddScope(string scope)
        {
            _scopes.Add(new ClientScope(this, scope));
        }

        public void AddGrantType(string grantType)
        {
            _grantTypes.Add(new ClientGrantType(this, grantType));
        }
    }

    public class ClientRedirectUri
    {
        public string ClientRedirectUriId { get; private set; }
        public string ClientId { get; private set; }
        public string RedirectUri { get; private set; }

        public Client Client { get; private set; }

        internal ClientRedirectUri()
        {
            
        }
        public ClientRedirectUri(Client client, string redirectUri)
        {
            ClientRedirectUriId = Guid.NewGuid().ToString();
            Client = client;
            ClientId = client.ClientId;
            RedirectUri = redirectUri;
        }
    }

    public class ClientScope
    {
        public string ClientScopeId { get; private set; }
        public string ClientId { get; private set; }
        public string Scope { get; private set; }

        public Client Client { get; private set; }

        internal ClientScope()
        {
            
        }
        public ClientScope(Client client, string scope)
        {
            ClientScopeId = Guid.NewGuid().ToString();
            Client = client;
            ClientId = client.ClientId;
            Scope = scope;
        }
    }

    public class ClientGrantType
    {
        public string ClientGrantTypeId { get; private set; }
        public string ClientId { get; private set; }
        public string GrantType { get; private set; }

        public Client Client { get; private set; }

        internal ClientGrantType()
        {
            
        }
        public ClientGrantType(Client client, string grantType)
        {
            ClientGrantTypeId = Guid.NewGuid().ToString();
            Client = client;
            ClientId = client.ClientId;
            GrantType = grantType;
        }
    }

    public class AuthorizationCode : IAggregateRoot
    {
        public string AuthorizationCodeId { get; private set; }
        public string Code { get; private set; }
        public string ClientId { get; private set; }
        public string UserId { get; private set; }
        public string RedirectUri { get; private set; }
        public string Scopes { get; private set; }
        public string CodeChallenge { get; private set; }
        public string CodeChallengeMethod { get; private set; }
        public DateTime Expiry { get; private set; }

        public User User { get; private set; }
        public Client Client { get; private set; }

        internal AuthorizationCode() { }

        public AuthorizationCode(Client client, User user, string redirectUri, string scopes, DateTime expiry, string codeChallenge, string codeChallengeMethod)
        {
            AuthorizationCodeId = Guid.NewGuid().ToString();
            Code = GenerateCode();
            Client = client;
            ClientId = client.ClientId;
            User = user;
            UserId = user.UserId;
            RedirectUri = redirectUri;
            Scopes = scopes;
            Expiry = expiry;
            CodeChallenge = codeChallenge;
            CodeChallengeMethod = codeChallengeMethod;
        }

        public bool IsExpired() => DateTime.UtcNow > Expiry;

        private string GenerateCode()
        {
            // Implement code generation logic
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }

    public class Token : IAggregateRoot
    {
        public string TokenId { get; private set; }
        public string TokenType { get; private set; } // Access or Refresh
        public string Value { get; private set; }
        public string ClientId { get; private set; }
        public string UserId { get; private set; }
        public DateTime Expiry { get; private set; }

        public User User { get; private set; }
        public Client Client { get; private set; }

        internal Token() { }
        public Token(Client client, User user, string tokenType, DateTime expiry, string value)
        {
            TokenId = Guid.NewGuid().ToString();
            TokenType = tokenType;
            Client = client;
            ClientId = client.ClientId;
            User = user;
            UserId = user.UserId;
            Expiry = expiry;
            Value = value;
        }

    }
}

