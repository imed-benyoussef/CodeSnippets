using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Aiglusoft.IAM.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public class JwksControllerTests : IClassFixture<AiglusoftWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public JwksControllerTests(AiglusoftWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("https://localhost")
            });
        }

        [Fact]
        public async Task Get_ShouldReturnJwks()
        {
            // Act
            var response = await _client.GetAsync(".well-known/jwks.json");

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
    }
}
