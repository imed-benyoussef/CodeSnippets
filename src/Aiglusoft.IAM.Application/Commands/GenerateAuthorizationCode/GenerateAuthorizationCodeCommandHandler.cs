using Aiglusoft.IAM.Domain.Entities;
using Aiglusoft.IAM.Domain.Repositories;
using MediatR;

namespace Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode
{
    public class GenerateAuthorizationCodeCommandHandler : IRequestHandler<GenerateAuthorizationCodeCommand, string>
    {
        private readonly IAuthorizationCodeRepository _authorizationCodeRepository;

        public GenerateAuthorizationCodeCommandHandler(IAuthorizationCodeRepository authorizationCodeRepository)
        {
            _authorizationCodeRepository = authorizationCodeRepository;
        }

        public async Task<string> Handle(GenerateAuthorizationCodeCommand request, CancellationToken cancellationToken)
        {
            var authorizationCode = new AuthorizationCode(request.ClientId);
            await _authorizationCodeRepository.SaveAsync(authorizationCode);
            return authorizationCode.Code;
        }
    }
}
