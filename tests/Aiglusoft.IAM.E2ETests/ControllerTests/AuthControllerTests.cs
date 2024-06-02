using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public class AuthControllerTests : BaseIntegrationTest
    {
        public AuthControllerTests(WebApplicationFactory<Aiglusoft.IAM.Server.Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnToken()
        {
            // Arrange
            var loginModel = new { Username = "testuser", Password = "password" };
            var content = CreateJsonContent(loginModel);

            // Act
            var response = await Client.PostAsync("/api/auth/token", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(responseString));
            var token = JsonConvert.DeserializeObject<TokenResponse>(responseString);
            Assert.False(string.IsNullOrEmpty(token.Token));
        }

        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }

}