using System.Security.Cryptography;
using System.Text;

namespace Aiglusoft.IAM.Domain.Model.ClientAggregates
{
  public class Client : IAggregateRoot
  {
    public string ClientId { get; private set; }
    public string ClientSecret { get; private set; }
    public string ClientName { get; private set; }

    private readonly List<ClientRedirectUri> _redirectUris;
    public IReadOnlyCollection<ClientRedirectUri> RedirectUris => _redirectUris.AsReadOnly();

    private readonly List<ClientScope> _scopes;
    public IReadOnlyCollection<ClientScope> Scopes => _scopes.AsReadOnly();

    private readonly List<ClientGrantType> _grantTypes;
    public IReadOnlyCollection<ClientGrantType> GrantTypes => _grantTypes.AsReadOnly();

    internal Client() { }

    public Client(string clientId, string clientSecret, string clientName)
    {
      if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentException("Client ID cannot be null or empty", nameof(clientId));
      if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentException("Client Secret cannot be null or empty", nameof(clientSecret));
      if (string.IsNullOrWhiteSpace(clientName)) throw new ArgumentException("Client Name cannot be null or empty", nameof(clientName));

      ClientId = clientId;
      ClientSecret = HashSecret(clientSecret);
      ClientName = clientName;
      _redirectUris = new List<ClientRedirectUri>();
      _scopes = new List<ClientScope>();
      _grantTypes = new List<ClientGrantType>();
    }

    private string HashSecret(string secret)
    {
      return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secret)));
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
}

