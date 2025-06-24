using System;
using System.Threading.Tasks;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.Services;
using Moq;
using Xunit;

namespace AICO.Domain.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _userService = new UserService(_userRepositoryMock.Object, _jwtServiceMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_WhenEmailAndUsernameAreUnique()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.IsEmailInUseAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.IsUsernameInUseAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);

            // Act
            var user = await _userService.RegisterAsync("test@example.com", "testuser", "Max", "Mustermann", "password123");

            // Assert
            Assert.NotNull(user);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("testuser", user.Username);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEmailExists()
        {
            _userRepositoryMock.Setup(r => r.IsEmailInUseAsync(It.IsAny<string>())).ReturnsAsync(true);
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _userService.RegisterAsync("test@example.com", "testuser", "Max", "Mustermann", "password123"));
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenUsernameExists()
        {
            _userRepositoryMock.Setup(r => r.IsEmailInUseAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.IsUsernameInUseAsync(It.IsAny<string>())).ReturnsAsync(true);
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _userService.RegisterAsync("test@example.com", "testuser", "Max", "Mustermann", "password123"));
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            // Arrange
            var salt = "somesalt";
            var password = "password123";
            var hash = typeof(UserService)
                .GetMethod("HashPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .Invoke(null, new object[] { password, salt }) as string;

            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", hash!, salt);
            _userRepositoryMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.AuthenticateAsync("test@example.com", password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldThrow_WhenUserNotFound()
        {
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _userService.AuthenticateAsync("notfound@example.com", "password"));
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldThrow_WhenPasswordIsWrong()
        {
            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", "hash", "salt");
            _userRepositoryMock.Setup(r => r.GetByEmailAsync("test@example.com")).ReturnsAsync(user);
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _userService.AuthenticateAsync("test@example.com", "wrongpassword"));
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
        {
            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", "hash", "salt");
            var id = Guid.NewGuid();
            typeof(User).GetProperty("Id")!.SetValue(user, id);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            var result = await _userService.GetByIdAsync(id);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _userService.GetByIdAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdateProfileAsync_ShouldUpdateAndReturnUser()
        {
            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", "hash", "salt");
            var id = Guid.NewGuid();
            typeof(User).GetProperty("Id")!.SetValue(user, id);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            var result = await _userService.UpdateProfileAsync(id, "Moritz", "M�ller", "newuser");

            Assert.Equal("newuser", result.Username);
            Assert.Equal("Moritz", result.FirstName);
            Assert.Equal("M�ller", result.LastName);
            _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateProfileAsync_ShouldThrow_WhenNotFound()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _userService.UpdateProfileAsync(Guid.NewGuid(), "a", "b", "c"));
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldUpdatePassword_WhenCurrentPasswordIsCorrect()
        {
            var salt = "somesalt";
            var password = "oldpassword";
            var hash = typeof(UserService)
                .GetMethod("HashPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .Invoke(null, new object[] { password, salt }) as string;

            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", hash!, salt);
            var id = Guid.NewGuid();
            typeof(User).GetProperty("Id")!.SetValue(user, id);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            await _userService.ChangePasswordAsync(id, password, "newpassword");

            _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrow_WhenUserNotFound()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _userService.ChangePasswordAsync(Guid.NewGuid(), "old", "new"));
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrow_WhenCurrentPasswordIsWrong()
        {
            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", "hash", "salt");
            var id = Guid.NewGuid();
            typeof(User).GetProperty("Id")!.SetValue(user, id);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _userService.ChangePasswordAsync(id, "wrong", "new"));
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldSetEmailVerified_WithValidToken()
        {
            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", "hash", "salt");
            var id = Guid.NewGuid();
            var validToken = "valid-token";
            typeof(User).GetProperty("Id")!.SetValue(user, id);
            typeof(User).GetProperty("VerificationToken")!.SetValue(user, validToken);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            await _userService.VerifyEmailAsync(id, validToken);

            Assert.True(user.IsEmailVerified);
            Assert.Null(user.VerificationToken);
            _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldThrow_WhenUserNotFound()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _userService.VerifyEmailAsync(Guid.NewGuid(), "token"));
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldThrow_WhenNoTokenFound()
        {
            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", "hash", "salt");
            var id = Guid.NewGuid();
            typeof(User).GetProperty("Id")!.SetValue(user, id);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.VerifyEmailAsync(id, "token"));
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldThrow_WhenInvalidToken()
        {
            var user = User.Create("test@example.com", "testuser", "Max", "Mustermann", "hash", "salt");
            var id = Guid.NewGuid();
            typeof(User).GetProperty("Id")!.SetValue(user, id);
            typeof(User).GetProperty("VerificationToken")!.SetValue(user, "stored-token");
            _userRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(user);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _userService.VerifyEmailAsync(id, "wrong-token"));
        }
    }
}