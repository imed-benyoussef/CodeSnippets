using Aiglusoft.IAM.Application.Queries;
using Aiglusoft.IAM.Application.Queries.GetDiscoveryDocument;
using Aiglusoft.IAM.Domain.Services;
using Aiglusoft.IAM.Domain.ValueObjects;
using Moq;

namespace Aiglusoft.IAM.Tests.Handlers
{
    public class GetDiscoveryDocumentQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnDiscoveryDocument()
        {
            // Arrange
            var discoveryServiceMock = new Mock<IDiscoveryService>();
            discoveryServiceMock.Setup(ds => ds.GetDiscoveryDocument()).Returns(new OidcDiscoveryDocument { Issuer = "https://example.com" });

            var handler = new GetDiscoveryDocumentQueryHandler(discoveryServiceMock.Object);

            // Act
            var result = await handler.Handle(new GetDiscoveryDocumentQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("https://example.com", result.Issuer);
        }
    }


}