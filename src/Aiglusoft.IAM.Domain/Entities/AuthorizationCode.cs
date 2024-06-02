namespace Aiglusoft.IAM.Domain.Entities
{
    public class AuthorizationCode
    {
        public string Code { get; private set; }
        public string ClientId { get; private set; }
        public DateTime Expiration { get; private set; }

        public AuthorizationCode(string clientId)
        {
            Code = Guid.NewGuid().ToString();
            ClientId = clientId;
            Expiration = DateTime.UtcNow.AddMinutes(10); // Example expiration time
        }

        public bool IsExpired() => DateTime.UtcNow > Expiration;
    }
}
