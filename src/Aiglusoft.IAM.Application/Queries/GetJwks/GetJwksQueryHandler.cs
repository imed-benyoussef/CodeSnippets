using MediatR;
using Aiglusoft.IAM.Infrastructure.Services;
using Aiglusoft.IAM.Domain;

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
