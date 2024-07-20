//using Microsoft.IdentityModel.Tokens;
//using System.Security.Cryptography;
//using System.Text.Json;

using Microsoft.IdentityModel.Tokens;
namespace Aiglusoft.IAM.Domain.Services
{
    public interface IJwksService
    {
        JsonWebKeySet GetJsonWebKeySet();
    }
}
