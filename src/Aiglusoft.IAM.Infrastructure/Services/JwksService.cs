using Aiglusoft.IAM.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using JsonWebKey = Aiglusoft.IAM.Domain.Services.JsonWebKey;
using JsonWebKeySet = Aiglusoft.IAM.Domain.Services.JsonWebKeySet;

namespace Aiglusoft.IAM.Infrastructure.Services
{
  public class JwksService : IJwksService
    {
        private readonly ICertificateService _certificateService;

        public JwksService(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        public JsonWebKeySet GetJsonWebKeySet()
        {
            var rsa = _certificateService.GetRsaPublicKey();
            var parameters = rsa.ExportParameters(false);
            var keyId = _certificateService.GetKeyId();

            var jwk = new JsonWebKey
            {
                Kty = "RSA",
                Use = "sig",
                Kid = keyId,
                E = Base64UrlEncoder.Encode(parameters.Exponent),
                N = Base64UrlEncoder.Encode(parameters.Modulus)
            };

            return new JsonWebKeySet
            {
                Keys = new[] { jwk }
            };
        }
    }
}
