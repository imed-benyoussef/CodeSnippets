using Aiglusoft.IAM.Application.Commands;
using Aiglusoft.IAM.Application.Commands.Handlers;
using Aiglusoft.IAM.Domain.Entities;
using Aiglusoft.IAM.Domain.Repositories;
using Moq;

namespace Aiglusoft.IAM.Tests.Handlers
{
    public class CreateUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var handler = new CreateUserCommandHandler(userRepositoryMock.Object);
            var command = new CreateUserCommand("testuser", "password");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        }
    }


}