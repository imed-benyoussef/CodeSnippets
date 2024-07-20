using System.Security.Cryptography;

namespace Aiglusoft.IAM.Domain.Services
{
    public interface IEncryptionService
    {
        string Decrypt(string encryptedMessageBase64);
        string Encrypt(string message, RSA publicKey);
        RSAParameters GetPublicKey();
    }

}
