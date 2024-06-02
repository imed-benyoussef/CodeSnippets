using MediatR;
using Aiglusoft.IAM.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Domain.Repositories;

namespace Aiglusoft.IAM.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.Username, HashPassword(request.Password));
            await _userRepository.AddAsync(user);
            return user.Id;
        }

        private string HashPassword(string password)
        {
            // Implement password hashing here
            return password;
        }
    }
}
