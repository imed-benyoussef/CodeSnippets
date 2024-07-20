using MediatR;
using Aiglusoft.IAM.Domain.Services;
using Microsoft.IdentityModel.Tokens;

namespace Aiglusoft.IAM.Application.Queries.GetJwks
{
    public class GetJwksQueryHandler : IRequestHandler<GetJwksQuery, JsonWebKeySet>
    {
        private readonly IJwksService _jwksService;

        public GetJwksQueryHandler(IJwksService jwksService)
        {
            _jwksService = jwksService;
        }

        public Task<JsonWebKeySet> Handle(GetJwksQuery request, CancellationToken cancellationToken)
        {
            var jwks = _jwksService.GetJsonWebKeySet();
            return Task.FromResult(jwks);
        }
    }
}
