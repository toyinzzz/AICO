using AICO.Domain.Entities;
using AICO.Domain.Interfaces;
using AICO.Domain.Services;
using Moq;
using Xunit;

namespace AICO.Domain.Tests.Services
{
    public class AnalysisServiceTests
    {
        private readonly Mock<IAuditService> _mockAuditService;
        private readonly Mock<IRecommendationService> _mockRecommendationService;
        private readonly IAnalysisService _analysisService;

        public AnalysisServiceTests()
        {
            _mockAuditService = new Mock<IAuditService>();
            _mockRecommendationService = new Mock<IRecommendationService>();
            _analysisService = new AnalysisService(_mockAuditService.Object, _mockRecommendationService.Object);
        }

        [Fact]
        public void CreateAnalysisEntity_ShouldReturnValidEntity()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var analysisType = "SEO";
            var score = 85;
            var resultData = "{\"key\":\"value\"}";
            var summary = "Test summary";

            // Act
            var result = _analysisService.CreateAnalysisEntity(websiteId, analysisType, score, resultData, summary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(websiteId, result.WebsiteId);
            Assert.Equal(analysisType, result.AnalysisType);
            Assert.Equal(score, result.Score);
            Assert.Equal(resultData, result.ResultData);
            Assert.Equal(summary, result.Summary);
            Assert.NotNull(result.Recommendations);
            Assert.Empty(result.Recommendations);
        }

        [Fact]
        public async Task CreateAnalysisAsync_ShouldCreateAndAuditEntity()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var analysisType = "Performance";
            var score = 75;
            var resultData = "{\"speed\":\"fast\"}";
            var summary = "Performance test";

            // Act
            var result = await _analysisService.CreateAnalysisAsync(websiteId, analysisType, score, resultData, summary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(websiteId, result.WebsiteId);
            Assert.Equal(analysisType, result.AnalysisType);
            Assert.Equal(score, result.Score);
            Assert.Equal(resultData, result.ResultData);
            Assert.Equal(summary, result.Summary);
            _mockAuditService.Verify(s => s.SetCreationAudit(It.IsAny<IAuditableEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAnalysisAsync_ShouldUpdateAndAuditEntity()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var analysis = AnalysisResult.Create(
                websiteId,
                "Accessibility",
                65,
                "{\"issues\":\"minor\"}",
                "Initial summary"
            );

            var newScore = 80;
            var newResultData = "{\"issues\":\"none\"}";
            var newSummary = "Updated summary";

            // Act
            await _analysisService.UpdateAnalysisAsync(analysis, newScore, newResultData, newSummary);

            // Assert
            Assert.Equal(newScore, analysis.Score);
            Assert.Equal(newResultData, analysis.ResultData);
            Assert.Equal(newSummary, analysis.Summary);
            _mockAuditService.Verify(s => s.UpdateModificationDate(It.IsAny<IAuditableEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddRecommendationAsync_ShouldAddRecommendationToAnalysis()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var analysis = AnalysisResult.Create(
                websiteId,
                "SEO",
                70,
                "{\"data\":\"test\"}",
                "Test analysis"
            );

            // Set ID for the analysis
            typeof(BaseEntity).GetProperty("Id").SetValue(analysis, Guid.NewGuid());

            var title = "Improve meta tags";
            var description = "Add better meta descriptions";
            var priority = 2;
            var category = "SEO";

            var recommendation = Recommendation.Create(
                analysis.Id,
                title,
                description,
                priority,
                category
            );

            _mockRecommendationService
                .Setup(s => s.CreateRecommendationAsync(
                    analysis.Id,
                    title,
                    description,
                    priority,
                    category
                ))
                .ReturnsAsync(recommendation);

            // Act
            await _analysisService.AddRecommendationAsync(analysis, title, description, priority, category);

            // Assert
            Assert.Contains(recommendation, analysis.Recommendations);
            Assert.Single(analysis.Recommendations);
            _mockAuditService.Verify(s => s.UpdateModificationDate(It.IsAny<IAuditableEntity>()), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task CreateAnalysisAsync_WithInvalidAnalysisType_ShouldThrowException(string invalidAnalysisType)
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var score = 85;
            var resultData = "{\"key\":\"value\"}";
            var summary = "Test summary";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _analysisService.CreateAnalysisAsync(websiteId, invalidAnalysisType, score, resultData, summary));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public async Task CreateAnalysisAsync_WithInvalidScore_ShouldThrowException(int invalidScore)
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var analysisType = "SEO";
            var resultData = "{\"key\":\"value\"}";
            var summary = "Test summary";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _analysisService.CreateAnalysisAsync(websiteId, analysisType, invalidScore, resultData, summary));
        }

        [Fact]
        public async Task ValidateAnalysisDataAsync_WithValidJson_ShouldReturnTrue()
        {
            // Arrange
            var validJson = "{\"key\":\"value\"}";

            // Act
            var result = await _analysisService.ValidateAnalysisDataAsync(validJson);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateAnalysisDataAsync_WithInvalidJson_ShouldReturnFalse()
        {
            // Arrange
            var invalidJson = "{key:value}";

            // Act
            var result = await _analysisService.ValidateAnalysisDataAsync(invalidJson);

            // Assert
            Assert.False(result);
        }
    }
}