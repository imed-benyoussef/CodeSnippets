using Microsoft.AspNetCore.Mvc.Testing;
using Aiglusoft.IAM.Server;
using Microsoft.Extensions.DependencyInjection;


namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public class JwksControllerTests : BaseIntegrationTest
    {
        public JwksControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_ShouldReturnJwks()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/.well-known/jwks.json");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("keys", responseString);
            Assert.Contains("kid", responseString);
            Assert.Contains("kty", responseString);
            Assert.Contains("use", responseString);
            Assert.Contains("n", responseString);
            Assert.Contains("e", responseString);
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            
        }
    }

}
