
namespace Aiglusoft.IAM.Tests.Services
{
    using Xunit;
    using Moq;
    using Microsoft.Extensions.Configuration;
    using Aiglusoft.IAM.Infrastructure.Services;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Hosting;
    using System.Security.Cryptography;

    public class CertificateServiceTests
    {
        [Fact]
        public void GetCertificate_ShouldReturnValidCertificate()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["Oidc:CertificatePath"]).Returns("certs/certificate.pfx");
            configurationMock.Setup(c => c["Oidc:CertificatePassword"]).Returns("");

            var environmentMock = new Mock<IWebHostEnvironment>();
            environmentMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var service = new CertificateService(configurationMock.Object);

            // Act
            var result = service.GetCertificate();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<X509Certificate2>(result);
        }

        [Fact]
        public void GetRsaPrivateKey_ShouldReturnPrivateKey()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["Oidc:CertificatePath"]).Returns("certs/certificate.pfx");
            configurationMock.Setup(c => c["Oidc:CertificatePassword"]).Returns("");

            var environmentMock = new Mock<IWebHostEnvironment>();
            environmentMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var service = new CertificateService(configurationMock.Object);

            // Act
            var result = service.GetRsaPrivateKey();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<RSA>(result);
        }

        [Fact]
        public void GetKeyId_ShouldReturnKeyId()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["Oidc:CertificatePath"]).Returns("certs/certificate.pfx");
            configurationMock.Setup(c => c["Oidc:CertificatePassword"]).Returns("");

            var environmentMock = new Mock<IWebHostEnvironment>();
            environmentMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var service = new CertificateService(configurationMock.Object);

            // Act
            var result = service.GetKeyId();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<RSA>(result);
        }


        [Fact]
        public void GetRsaPublicKey_ShouldReturnPublicKey()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["Oidc:CertificatePath"]).Returns("certs/certificate.pfx");
            configurationMock.Setup(c => c["Oidc:CertificatePassword"]).Returns("");

            var environmentMock = new Mock<IWebHostEnvironment>();
            environmentMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var service = new CertificateService(configurationMock.Object);

            // Act
            var result = service.GetRsaPublicKey();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<RSA>(result);
        }
    }
}
