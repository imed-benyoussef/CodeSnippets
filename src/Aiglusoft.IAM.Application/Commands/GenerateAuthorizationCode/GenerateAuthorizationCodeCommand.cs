using MediatR;

namespace Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode
{
    public class GenerateAuthorizationCodeCommand : IRequest<string>
    {
        public string ClientId { get; }
        public string RedirectUri { get; }
        public string State { get; }

        public GenerateAuthorizationCodeCommand(string clientId, string redirectUri, string state)
        {
            ClientId = clientId;
            RedirectUri = redirectUri;
            State = state;
        }
    }

}
