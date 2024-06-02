using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Domain.ValueObjects;
using MediatR;

namespace Aiglusoft.IAM.Application.Queries.GetDiscoveryDocument
{
    public class GetDiscoveryDocumentQueryHandler : IRequestHandler<GetDiscoveryDocumentQuery, OidcDiscoveryDocument>
    {
        private readonly IDiscoveryService _discoveryService;

        public GetDiscoveryDocumentQueryHandler(IDiscoveryService discoveryService)
        {
            _discoveryService = discoveryService;
        }

        public Task<OidcDiscoveryDocument> Handle(GetDiscoveryDocumentQuery request, CancellationToken cancellationToken)
        {
            var discoveryDocument = _discoveryService.GetDiscoveryDocument();
            return Task.FromResult(discoveryDocument);
        }
    }
}
