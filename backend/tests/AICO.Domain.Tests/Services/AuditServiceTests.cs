using AICO.Domain.Entities;
using AICO.Domain.Interfaces;
using AICO.Domain.Services;
using Xunit;

namespace AICO.Domain.Tests.Services
{
    public class AuditServiceTests
    {
        private readonly IAuditService _auditService;

        public AuditServiceTests()
        {
            _auditService = new AuditService();
        }

        [Fact]
        public void UpdateModificationDate_WithValidEntity_SetsModificationDate()
        {
            // Arrange
            var user = User.Create(
                "test@example.com",
                "testuser",
                "Test",
                "User",
                "hashedpassword",
                "salt");

            // Act
            _auditService.UpdateModificationDate(user);

            // Assert
            Assert.NotNull(user.ModifiedAt);
            Assert.True(user.ModifiedAt > DateTime.UtcNow.AddMinutes(-1));
            Assert.True(user.ModifiedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void UpdateModificationDate_WithUserId_SetsModifiedBy()
        {
            // Arrange
            var user = User.Create(
                "test@example.com",
                "testuser",
                "Test",
                "User",
                "hashedpassword",
                "salt");

            // Act
            _auditService.UpdateModificationDate(user, "admin");

            // Assert
            Assert.Equal("admin", user.ModifiedBy);
            Assert.NotNull(user.ModifiedAt);
            Assert.True(user.ModifiedAt > DateTime.UtcNow.AddMinutes(-1));
            Assert.True(user.ModifiedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void UpdateModificationDate_WithNullEntity_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _auditService.UpdateModificationDate(null));

            Assert.Equal("entity", exception.ParamName);
        }

        [Fact]
        public void SetCreationAudit_WithValidEntity_SetsCreationDate()
        {
            // Arrange
            var user = User.Create(
                "test@example.com",
                "testuser",
                "Test",
                "User",
                "hashedpassword",
                "salt");

            // Act
            _auditService.SetCreationAudit(user);

            // Assert
            Assert.NotNull(user.CreatedAt);
            Assert.True(user.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
            Assert.True(user.CreatedAt <= DateTime.UtcNow);
            Assert.Equal("System", user.CreatedBy);
        }

        [Fact]
        public void SetCreationAudit_WithUserId_SetsCreatedBy()
        {
            // Arrange
            var user = User.Create(
                "test@example.com",
                "testuser",
                "Test",
                "User",
                "hashedpassword",
                "salt");

            // Act
            _auditService.SetCreationAudit(user, "admin");

            // Assert
            Assert.Equal("admin", user.CreatedBy);
            Assert.NotNull(user.CreatedAt);
            Assert.True(user.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
            Assert.True(user.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void SetCreationAudit_WithNullEntity_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _auditService.SetCreationAudit(null));

            Assert.Equal("entity", exception.ParamName);
        }
    }
}