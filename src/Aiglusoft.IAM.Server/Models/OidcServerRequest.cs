namespace Aiglusoft.IAM.Server.Models
{
    public class OidcServerRequest
    {
        // Properties for AuthorizeRequest
        public string ResponseType { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string CodeChallenge { get; set; }
        public string CodeChallengeMethod { get; set; }
        public string State { get; set; }
        public string Nonce { get; set; }
        public string Scope { get; set; }

        // Properties for TokenRequest
        public string Code { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string Prompt { get; internal set; }
        public string MaxAge { get; internal set; }
        public string Display { get; internal set; }
        public string AcrValues { get; internal set; }
        public string IdTokenHint { get; internal set; }
        public string LoginHint { get; internal set; }
    }
}