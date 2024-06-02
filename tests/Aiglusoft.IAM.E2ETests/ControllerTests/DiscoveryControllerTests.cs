using Aiglusoft.IAM.Server;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public class DiscoveryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public DiscoveryControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ShouldReturnDiscoveryDocument()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/.well-known/openid-configuration");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("issuer", responseString);
        }
    }

}