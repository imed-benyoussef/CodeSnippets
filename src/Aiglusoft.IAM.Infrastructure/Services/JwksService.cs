using Aiglusoft.IAM.Domain;
using Aiglusoft.IAM.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using JsonWebKey = Aiglusoft.IAM.Domain.JsonWebKey;
using JsonWebKeySet = Aiglusoft.IAM.Domain.JsonWebKeySet;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class JwksService : IJwksService
    {
        private readonly ICertificateService _certificateService;
        private readonly IConfiguration _configuration;

        public JwksService(ICertificateService certificateService, IConfiguration configuration)
        {
            _certificateService = certificateService;
            _configuration = configuration;
        }

        public JsonWebKeySet GetJsonWebKeySet()
        {
            var rsa = _certificateService.GetRsaPublicKey();
            var parameters = rsa.ExportParameters(false);
            var keyId = _configuration["Oidc:KeyId"];

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
