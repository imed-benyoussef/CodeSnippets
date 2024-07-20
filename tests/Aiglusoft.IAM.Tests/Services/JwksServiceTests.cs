
namespace Aiglusoft.IAM.Tests.Services
{
  using Xunit;
  using Moq;
  using Aiglusoft.IAM.Infrastructure.Services;
  using Microsoft.IdentityModel.Tokens;
  using System.Security.Cryptography;
  using Aiglusoft.IAM.Domain.Services;

  public class JwksServiceTests
    {
        [Fact]
        public void GetJsonWebKeySet_ShouldReturnValidJwks()
        {
            // Arrange
            var certificateServiceMock = new Mock<ICertificateService>();

            var rsa = RSA.Create();
            var parameters = rsa.ExportParameters(false);
            certificateServiceMock.Setup(cs => cs.GetRsaPublicKey()).Returns(rsa);
            certificateServiceMock.Setup(cs => cs.GetKeyId()).Returns("your-key-id");

            var service = new JwksService(certificateServiceMock.Object);

            // Act
            var result = service.GetJsonWebKeySet();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Keys);
            var key = result.Keys.FirstOrDefault();
            Assert.Equal("RSA", key.Kty);
            Assert.Equal("sig", key.Use);
            Assert.Equal("your-key-id", key.Kid);
            Assert.Equal(Base64UrlEncoder.Encode(parameters.Exponent), key.E);
            Assert.Equal(Base64UrlEncoder.Encode(parameters.Modulus), key.N);
        }
    }

}
