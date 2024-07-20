namespace Aiglusoft.IAM.Domain.Model.ClientAggregates
{
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
}

