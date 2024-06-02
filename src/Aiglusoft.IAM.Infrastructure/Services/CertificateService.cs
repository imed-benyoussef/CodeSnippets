using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Aiglusoft.IAM.Domain;

namespace Aiglusoft.IAM.Infrastructure.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IConfiguration _configuration;
        private readonly X509Certificate2 _certificate;

        public CertificateService(IConfiguration configuration)
        {
            _configuration = configuration;
            _certificate = new X509Certificate2(
                _configuration["Oidc:CertificatePath"],
                _configuration["Oidc:CertificatePassword"]);

            if (!_certificate.HasPrivateKey)
            {
                throw new CryptographicException("Le certificat ne contient pas de clé privée.");
            }
        }

        public X509Certificate2 GetCertificate()
        {
            return _certificate;
        }

        public RSA GetRsaPrivateKey()
        {
            return _certificate.GetRSAPrivateKey();
        }

        public RSA GetRsaPublicKey()
        {
            return _certificate.GetRSAPublicKey();
        }

        public string GetKeyId()
        {
            return _certificate.Thumbprint;
        }
    }
}
