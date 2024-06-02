
namespace Aiglusoft.IAM.Tests.Services
{
    using Xunit;
    using Moq;
    using Microsoft.Extensions.Configuration;
    using Aiglusoft.IAM.Domain.Services;
    using Aiglusoft.IAM.Infrastructure.Services;

    public class DiscoveryServiceTests
    {
        [Fact]
        public void GetDiscoveryDocument_ShouldReturnValidDocument()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["Oidc:Issuer"]).Returns("https://example.com");

            var service = new DiscoveryService(configurationMock.Object);

            // Act
            var result = service.GetDiscoveryDocument();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("https://example.com", result.Issuer);
        }
    }
}
