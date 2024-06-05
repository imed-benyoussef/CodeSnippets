using MediatR;

namespace Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode
{
    public class GenerateAuthorizationCodeCommand : IRequest<string>
    {
        public string ClientId { get; }
        public string RedirectUri { get; }
        public string State { get; }
        public string ResponseType { get; }
        public string Scope { get; }
        public string Nonce { get; }
        public string CodeChallenge { get; }
        public string CodeChallengeMethod { get; }

        public GenerateAuthorizationCodeCommand(string clientId, string redirectUri, string state, string responseType, string scope, string nonce, string codeChallenge, string codeChallengeMethod)
        {
            ClientId = clientId;
            RedirectUri = redirectUri;
            State = state;
            ResponseType = responseType;
            Scope = scope;
            Nonce = nonce;
            CodeChallenge = codeChallenge;
            CodeChallengeMethod = codeChallengeMethod;
        }
    }

}
