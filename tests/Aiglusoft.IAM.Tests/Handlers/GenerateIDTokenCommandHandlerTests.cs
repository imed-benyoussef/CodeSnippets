//using Aiglusoft.IAM.Application.Commands;
//using Aiglusoft.IAM.Application.Commands.Handlers;
//using Microsoft.Extensions.Configuration;

//namespace Aiglusoft.IAM.Tests.Handlers
//{
//    public class GenerateIDTokenCommandHandlerTests
//    {
//        [Fact]
//        public async Task Handle_ShouldReturnIDToken()
//        {
//            // Arrange
//            var inMemorySettings = new Dictionary<string, string> {
//                {"Jwt:Issuer", "http://localhost:5193"},
//                {"Jwt:Audience", "http://localhost:5193"},
//                {"Jwt:Secret", "q1N3vXPf+a2u2rQ8nC2XhA5pMlN3V1E6sZrJqL7gPbY="}
//            };

//            IConfiguration configuration = new ConfigurationBuilder()
//                .AddInMemoryCollection(inMemorySettings)
//                .Build();

//            var handler = new GenerateIDTokenCommandHandler(configuration);
//            var command = new GenerateIDTokenCommand("testuser");

//            // Act
//            var result = await handler.Handle(command, CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Contains("eyJ", result); // Verify that the token is in JWT format
//        }
//    }

//}