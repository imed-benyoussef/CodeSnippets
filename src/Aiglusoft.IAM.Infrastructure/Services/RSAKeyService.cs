using System.Security.Cryptography;
using System.Text.Json;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class RSAKeyService
    {
        private readonly RSA _rsa;

        public RSAKeyService()
        {
            _rsa = RSA.Create(2048);
        }

        public RSAParameters GetPublicKeyParameters()
        {
            return _rsa.ExportParameters(false);
        }

        public byte[] DecryptData(byte[] encryptedData)
        {
            return _rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
        }

        public string GetJWKS()
        {
            var parameters = _rsa.ExportParameters(false);
            var jwk = new
            {
                kty = "RSA",
                n = Convert.ToBase64String(parameters.Modulus),
                e = Convert.ToBase64String(parameters.Exponent),
                alg = "RS256",
                use = "enc",
                kid = "1"
            };
            var jwks = new { keys = new[] { jwk } };
            return JsonSerializer.Serialize(jwks);
        }
    }
}
