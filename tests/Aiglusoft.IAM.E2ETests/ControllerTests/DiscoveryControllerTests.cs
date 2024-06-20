using Aiglusoft.IAM.Server;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public class DiscoveryControllerTests : IClassFixture<AiglusoftWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public DiscoveryControllerTests(AiglusoftWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("https://localhost")
            });
        }

        [Fact]
        public async Task Get_ShouldReturnDiscoveryDocument()
        {
            

            // Act
            var response = await _client.GetAsync("/.well-known/openid-configuration");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("issuer", responseString);
        }
    }

}