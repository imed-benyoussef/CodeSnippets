using Aiglusoft.IAM.Domain.ValueObjects;

namespace Aiglusoft.IAM.Domain.Services
{
    public interface IDiscoveryService
    {
        OidcDiscoveryDocument GetDiscoveryDocument();
    }
}
