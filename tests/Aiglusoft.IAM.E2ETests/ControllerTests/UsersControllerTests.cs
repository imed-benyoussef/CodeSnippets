using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{

    public class UsersControllerTests : BaseIntegrationTest
    {
        public UsersControllerTests(WebApplicationFactory<Aiglusoft.IAM.Server.Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task CreateUser_ShouldReturnSuccess()
        {
            // Arrange
            var command = new { Username = "testuser", Password = "password" };
            var content = CreateJsonContent(command);
            var client = Factory.CreateClient();
            // Act
            var response = await client.PostAsync("/api/users", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(responseString));
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
           
        }
    }

}