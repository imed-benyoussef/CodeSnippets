namespace Aiglusoft.IAM.Domain.Model.ClientAggregates
{
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
}

