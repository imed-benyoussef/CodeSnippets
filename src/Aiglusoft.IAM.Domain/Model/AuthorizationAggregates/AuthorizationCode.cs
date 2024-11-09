using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Domain.Model.UserAggregates;

namespace Aiglusoft.IAM.Domain.Model.AuthorizationAggregates
{
  public class AuthorizationCode : IAggregateRoot
  {
    public string Id { get; private set; }
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
      Id = Guid.NewGuid().ToString();
      Code = GenerateCode();
      Client = client;
      ClientId = client.ClientId;
      User = user;
      UserId = user.Id;
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
}

