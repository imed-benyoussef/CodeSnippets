using Aiglusoft.IAM.Application.Commands.GenerateAuthorizationCode;
using Aiglusoft.IAM.Domain.Entities;
using Aiglusoft.IAM.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Aiglusoft.IAM.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Aiglusoft.IAM.Application.Exceptions;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class GenerateAuthorizationCodeCommandHandlerTests
    {
        private readonly Mock<IAuthorizationCodeRepository> _mockAuthorizationCodeRepository;
        private readonly List<IValidator<GenerateAuthorizationCodeCommand>> _validators;
        private readonly GenerateAuthorizationCodeCommandHandler _handler;

        public GenerateAuthorizationCodeCommandHandlerTests()
        {
            _mockAuthorizationCodeRepository = new Mock<IAuthorizationCodeRepository>();
            _validators = new List<IValidator<GenerateAuthorizationCodeCommand>>();
            _handler = new GenerateAuthorizationCodeCommandHandler(_mockAuthorizationCodeRepository.Object, _validators);
        }

        [Fact]
        public async Task Handle_ShouldReturnRedirectUrl_WithAuthorizationCodeAndState()
        {
            // Arrange
            var command = new GenerateAuthorizationCodeCommand(
                "client_id",
                "http://localhost/callback",
                "random_state",
                "code",
                "openid",
                null,
                null,
                null
            );
            var cancellationToken = new CancellationToken();
            _mockAuthorizationCodeRepository.Setup(repo => repo.SaveAsync(It.IsAny<AuthorizationCode>(), cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().Contain("http://localhost/callback");
            result.Should().NotContain(":80");  // Ensure port 80 is not included
            result.Should().Contain("code=");
            result.Should().Contain("state=random_state");
        }

        [Fact]
        public async Task Handle_ShouldReturnRedirectUrl_WithIdTokenAndState_ForOidc()
        {
            // Arrange
            var command = new GenerateAuthorizationCodeCommand(
                "client_id",
                "https://localhost/callback",
                "random_state",
                "id_token",
                "openid",
                "random_nonce",
                null,
                null
            );
            var cancellationToken = new CancellationToken();
            _mockAuthorizationCodeRepository.Setup(repo => repo.SaveAsync(It.IsAny<AuthorizationCode>(), cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().Contain("https://localhost/callback");
            result.Should().NotContain(":443");  // Ensure port 443 is not included
            result.Should().Contain("id_token=");
            result.Should().Contain("state=random_state");
            result.Should().Contain("nonce=random_nonce");
        }

        [Fact]
        public async Task Handle_ShouldThrowOAuthException_WhenValidationFails()
        {
            // Arrange
            var command = new GenerateAuthorizationCodeCommand(
                "client_id",
                "http://localhost/callback",
                "random_state",
                "code",
                "openid",
                null,
                null,
                null
            );
            var cancellationToken = new CancellationToken();

            var validatorMock = new Mock<IValidator<GenerateAuthorizationCodeCommand>>();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GenerateAuthorizationCodeCommand>>(), cancellationToken))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                new ValidationFailure("ClientId", "ClientId is required") { ErrorCode = "invalid_request" }
                }));

            _validators.Add(validatorMock.Object);

            // Act
            Func<Task> act = () => _handler.Handle(command, cancellationToken);

            // Assert
            var exception = await Assert.ThrowsAsync<OAuthException>(act);
            exception.Error.Should().Be("invalid_request");
            exception.ErrorDescription.Should().Be("ClientId is required");
        }

        [Fact]
        public async Task Handle_ShouldNotThrowException_WhenValidationSucceeds()
        {
            // Arrange
            var command = new GenerateAuthorizationCodeCommand(
                "client_id",
                "http://localhost/callback",
                "random_state",
                "code",
                "openid",
                null,
                null,
                null
            );
            var cancellationToken = new CancellationToken();

            var validatorMock = new Mock<IValidator<GenerateAuthorizationCodeCommand>>();
            validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<GenerateAuthorizationCodeCommand>>(), cancellationToken))
                .ReturnsAsync(new ValidationResult());

            _validators.Add(validatorMock.Object);

            _mockAuthorizationCodeRepository.Setup(repo => repo.SaveAsync(It.IsAny<AuthorizationCode>(), cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = () => _handler.Handle(command, cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }


}