using MediatR;

namespace Aiglusoft.IAM.Application.Commands
{
    public class ExchangeAuthorizationCodeCommand : IRequest<ExchangeAuthorizationCodeCommand.TokenResponse>
    {
        public string Code { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string RedirectUri { get; }
        public string GrantType { get; }

        public ExchangeAuthorizationCodeCommand(string code, string clientId, string clientSecret, string redirectUri, string grantType)
        {
            Code = code;
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
            GrantType = grantType;
        }
        public class TokenResponse
        {
            public string IdToken { get; set; }
            public string AccessToken { get; set; }
            public string TokenType { get; set; }
            public int ExpiresIn { get; set; }
        }
    }

}
