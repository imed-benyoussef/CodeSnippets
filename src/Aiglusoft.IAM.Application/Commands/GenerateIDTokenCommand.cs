using MediatR;

namespace Aiglusoft.IAM.Application.Commands
{
    public class GenerateIDTokenCommand : IRequest<string>
    {
        public string Username { get; }

        public GenerateIDTokenCommand(string username)
        {
            Username = username;
        }
    }

}
