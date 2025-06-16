using AICO.Domain.Entities;
using AICO.Domain.Interfaces;
using AICO.Domain.Services;
using Moq;
using Xunit;

namespace AICO.Domain.Tests.Services
{
    public class RecommendationServiceTests
    {
        private readonly Mock<IAuditService> _mockAuditService;
        private readonly IRecommendationService _recommendationService;

        public RecommendationServiceTests()
        {
            _mockAuditService = new Mock<IAuditService>();
            _recommendationService = new RecommendationService(_mockAuditService.Object);
        }

        [Fact]
        public void CreateRecommendationEntity_ShouldReturnValidEntity()
        {
            // Arrange
            var analysisResultId = Guid.NewGuid();
            var title = "Improve meta tags";
            var description = "Add better meta descriptions";
            var priority = 2;
            var category = "SEO";

            // Act
            var result = _recommendationService.CreateRecommendationEntity(analysisResultId, title, description, priority, category);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(analysisResultId, result.AnalysisResultId);
            Assert.Equal(title, result.Title);
            Assert.Equal(description, result.Description);
            Assert.Equal(priority, result.Priority);
            Assert.Equal(category, result.Category);
            Assert.False(result.IsImplemented);
            Assert.Null(result.ImplementedAt);
        }

        [Fact]
        public async Task CreateRecommendationAsync_ShouldCreateAndAuditEntity()
        {
            // Arrange
            var analysisResultId = Guid.NewGuid();
            var title = "Add alt tags";
            var description = "Add alt tags to images";
            var priority = 1;
            var category = "Accessibility";

            // Act
            var result = await _recommendationService.CreateRecommendationAsync(analysisResultId, title, description, priority, category);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(analysisResultId, result.AnalysisResultId);
            Assert.Equal(title, result.Title);
            Assert.Equal(description, result.Description);
            Assert.Equal(priority, result.Priority);
            Assert.Equal(category, result.Category);
            Assert.False(result.IsImplemented);
            Assert.Null(result.ImplementedAt);
            _mockAuditService.Verify(s => s.SetCreationAudit(It.IsAny<IAuditableEntity>()), Times.Once);
        }

         [Fact]
        public async Task UpdateRecommendationAsync_ShouldUpdateAndAuditEntity()
        {
            // Arrange
            var analysisResultId = Guid.NewGuid();
            var recommendation = Recommendation.Create(
                analysisResultId,
                "Initial title",
                "Initial description",
                3,
                "Initial category"
            );

            var newTitle = "Updated title";
            var newDescription = "Updated description";
            var newPriority = 2;
            var newCategory = "Updated category";

            // Act
            await _recommendationService.UpdateRecommendationAsync(recommendation, newTitle, newDescription, newPriority, newCategory);

            // Assert
            Assert.Equal(newTitle, recommendation.Title);
            Assert.Equal(newDescription, recommendation.Description);
            Assert.Equal(newPriority, recommendation.Priority);
            Assert.Equal(newCategory, recommendation.Category);
            _mockAuditService.Verify(s => s.UpdateModificationDate(It.IsAny<IAuditableEntity>()), Times.Once);
        }

        [Fact]
        public async Task MarkAsImplementedAsync_ShouldUpdateImplementationStatusAndAudit()
        {
            // Arrange
            var analysisResultId = Guid.NewGuid();
            var recommendation = Recommendation.Create(
                analysisResultId,
                "Test recommendation",
                "Test description",
                2,
                "Test category"
            );

            // Act
            await _recommendationService.MarkAsImplementedAsync(recommendation);

            // Assert
            Assert.True(recommendation.IsImplemented);
            Assert.NotNull(recommendation.ImplementedAt);
            _mockAuditService.Verify(s => s.UpdateModificationDate(It.IsAny<IAuditableEntity>()), Times.Once);
        }

        [Fact]
        public async Task MarkAsNotImplementedAsync_ShouldUpdateImplementationStatusAndAudit()
        {
            // Arrange
            var analysisResultId = Guid.NewGuid();
            var implementedAt = DateTime.UtcNow;
            var recommendation = Recommendation.CreateImplemented(
                analysisResultId,
                "Test recommendation",
                "Test description",
                2,
                "Test category",
                implementedAt
            );

            // Act
            await _recommendationService.MarkAsNotImplementedAsync(recommendation);

            // Assert
            Assert.False(recommendation.IsImplemented);
            Assert.Null(recommendation.ImplementedAt);
            _mockAuditService.Verify(s => s.UpdateModificationDate(It.IsAny<IAuditableEntity>()), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task ValidateRecommendationDataAsync_WithInvalidTitle_ShouldReturnFalse(string invalidTitle)
        {
            // Arrange
            var priority = 2;

            // Act
            var result = await _recommendationService.ValidateRecommendationDataAsync(invalidTitle, priority);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public async Task ValidateRecommendationDataAsync_WithInvalidPriority_ShouldReturnFalse(int invalidPriority)
        {
            // Arrange
            var title = "Valid title";

            // Act
            var result = await _recommendationService.ValidateRecommendationDataAsync(title, invalidPriority);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateRecommendationDataAsync_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            var title = "Valid title";
            var priority = 3;

            // Act
            var result = await _recommendationService.ValidateRecommendationDataAsync(title, priority);

            // Assert
            Assert.True(result);
        }
    }
}