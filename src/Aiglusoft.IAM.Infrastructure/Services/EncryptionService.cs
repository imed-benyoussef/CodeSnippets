using Aiglusoft.IAM.Domain.Services;
using System.Security.Cryptography;
using System.Text;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly RSA _privateKey;

        public EncryptionService()
        {
            _privateKey = RSA.Create(2048);
        }

        public string Encrypt(string message, RSA publicKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            byte[] encryptedData = publicKey.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(encryptedData);
        }

        public string Decrypt(string encryptedMessageBase64)
        {
            byte[] encryptedData = Convert.FromBase64String(encryptedMessageBase64);
            byte[] decryptedData = _privateKey.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(decryptedData);
        }

        public RSAParameters GetPublicKey()
        {
            return _privateKey.ExportParameters(false);
        }

        public RSA GetPrivateKey()
        {
            return _privateKey;
        }
    }
}
