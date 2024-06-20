
namespace Aiglusoft.IAM.Tests.Services
{
    using Xunit;
    using Aiglusoft.IAM.Infrastructure.Services;

    public class HashPasswordServiceTests
    {
        private readonly HashPasswordService _hashPasswordService;

        public HashPasswordServiceTests()
        {
            _hashPasswordService = new HashPasswordService();
        }

        [Fact]
        public void HashPassword_ShouldReturnHash()
        {
            // Arrange
            var password = "password123";
            var securityStamp = "securitystamp";

            // Act
            var hashedPassword = _hashPasswordService.HashPassword(password, securityStamp);

            // Assert
            Assert.False(string.IsNullOrEmpty(hashedPassword));
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var password = "password123";
            var securityStamp = "securitystamp";
            var hashedPassword = _hashPasswordService.HashPassword(password, securityStamp);

            // Act
            var result = _hashPasswordService.VerifyPassword(password, hashedPassword, securityStamp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var password = "password123";
            var incorrectPassword = "wrongpassword";
            var securityStamp = "securitystamp";
            var hashedPassword = _hashPasswordService.HashPassword(password, securityStamp);

            // Act
            var result = _hashPasswordService.VerifyPassword(incorrectPassword, hashedPassword, securityStamp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenSecurityStampIsIncorrect()
        {
            // Arrange
            var password = "password123";
            var securityStamp = "securitystamp";
            var incorrectSecurityStamp = "wrongstamp";
            var hashedPassword = _hashPasswordService.HashPassword(password, securityStamp);

            // Act
            var result = _hashPasswordService.VerifyPassword(password, hashedPassword, incorrectSecurityStamp);

            // Assert
            Assert.False(result);
        }
    }

}
