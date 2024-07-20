namespace Aiglusoft.IAM.Domain.Model.ClientAggregates
{
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
}

