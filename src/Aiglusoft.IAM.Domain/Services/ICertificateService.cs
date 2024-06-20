using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System;

namespace Aiglusoft.IAM.Domain
{
    public interface ICertificateService
    {
        X509Certificate2 GetCertificate();
        RSA GetRsaPrivateKey();
        RSA GetRsaPublicKey();
        string GetKeyId();
    }

}
