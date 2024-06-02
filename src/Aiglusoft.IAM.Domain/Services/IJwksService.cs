using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Services;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public interface IJwksService
    {
        JsonWebKeySet GetJsonWebKeySet();
    }
}
