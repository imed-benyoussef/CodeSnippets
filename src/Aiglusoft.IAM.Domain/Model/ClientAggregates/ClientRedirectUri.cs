namespace Aiglusoft.IAM.Domain.Model.ClientAggregates
{
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
}

