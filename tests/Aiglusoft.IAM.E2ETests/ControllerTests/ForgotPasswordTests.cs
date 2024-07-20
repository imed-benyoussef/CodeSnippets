using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Aiglusoft.IAM.Server;
using FluentAssertions;
using Xunit;

namespace Aiglusoft.IAM.E2ETests.ControllerTests
{
    public class ForgotPasswordTests : AiglusoftE2ETestBase
    {
        public ForgotPasswordTests(ControllerTests.AiglusoftWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ForgotPassword_ShouldSendEmail()
        {

            // Arrange
            var email = "testuser@example.com";
            var content = new StringContent($"{{\"email\":\"{email}\"}}", Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync("/api/v1/account/forgot-password", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Password reset email sent.");
        }
    }
}
