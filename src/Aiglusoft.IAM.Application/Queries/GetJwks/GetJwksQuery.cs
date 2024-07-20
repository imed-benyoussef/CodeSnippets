using MediatR;
using Aiglusoft.IAM.Domain.Services;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Aiglusoft.IAM.Application.Queries.GetJwks
{
    public class GetJwksQuery : IRequest<JsonWebKeySet>
    {
    }
}
