using Aiglusoft.IAM.Domain.Model.ClientAggregates;
using Aiglusoft.IAM.Domain.Model.UserAggregates;

namespace Aiglusoft.IAM.Domain.Model.TokenAggregate
{
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
            UserId = user.Id;
            Expiry = expiry;
            Value = value;
        }

    }
}

