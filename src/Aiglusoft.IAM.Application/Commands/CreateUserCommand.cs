using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Application.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Username { get; }
        public string Password { get; }

        public CreateUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

}
