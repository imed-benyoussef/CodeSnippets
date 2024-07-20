using Aiglusoft.IAM.Domain.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
//using JsonWebKey = Aiglusoft.IAM.Domain.Services.JsonWebKey;
//using JsonWebKeySet = Aiglusoft.IAM.Domain.Services.JsonWebKeySet;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class JwksService : IJwksService
    {
        private readonly ICertificateService _certificateService;

        public JwksService(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        public  JsonWebKeySet GetJsonWebKeySet()
        {
            var rsa = _certificateService.GetRsaPublicKey();
            var parameters = rsa.ExportParameters(false);
            var keyId = _certificateService.GetKeyId();

            var jwk = new Microsoft.IdentityModel.Tokens.JsonWebKey
            {
                Kty = "RSA",
                Use = "sig",
                Kid = keyId,
                E = Base64UrlEncoder.Encode(parameters.Exponent),
                N = Base64UrlEncoder.Encode(parameters.Modulus),
                Alg = SecurityAlgorithms.RsaSha256
            };
            var d = new JsonWebKeySet();
            d.Keys.Add(jwk);
            return d;
        }
    }
}
