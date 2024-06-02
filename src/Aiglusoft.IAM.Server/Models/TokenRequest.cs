namespace Aiglusoft.IAM.Server.Models
{
    public class TokenRequest
    {
        public string Code { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string GrantType { get; set; }
    }
}