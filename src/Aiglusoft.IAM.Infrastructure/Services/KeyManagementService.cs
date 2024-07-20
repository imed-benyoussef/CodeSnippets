using System.Security.Cryptography;
//using JsonWebKey = Aiglusoft.IAM.Domain.Services.JsonWebKey;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    using System.Security.Cryptography;
    using System.Text;

    public class KeyManagementService
    {
        private RSAParameters _privateKeyParams;
        public RSAParameters PublicKeyParams { get; private set; }

        public KeyManagementService()
        {
            using (var rsa = RSA.Create(2048))
            {
                _privateKeyParams = rsa.ExportParameters(true);
                PublicKeyParams = rsa.ExportParameters(false);
            }
        }

        public string DecryptData(byte[] data)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(_privateKeyParams);
                var decryptedBytes = rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public string Base64UrlEncode(byte[] arg)
        {
            string s = Convert.ToBase64String(arg);
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-').Replace('/', '_');
            return s;
        }
    }

}
