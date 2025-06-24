using AICO.Domain.Entities;
using AICO.Domain.Interfaces;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.Services;
using Moq;
using Xunit;

namespace AICO.Domain.Tests.Services
{
    public class AnalysisServiceTests
    {
        private readonly Mock<IAuditService> _mockAuditService;
        private readonly Mock<IRecommendationService> _mockRecommendationService;
        private readonly Mock<IAnalysisResultRepository> _mockAnalysisResultRepository;
        private readonly Mock<IRecommendationRepository> _mockRecommendationRepository;
        private readonly IAnalysisService _service;

        public AnalysisServiceTests()
        {
            _mockAuditService = new Mock<IAuditService>();
            _mockRecommendationService = new Mock<IRecommendationService>();
            _mockAnalysisResultRepository = new Mock<IAnalysisResultRepository>();
            _mockRecommendationRepository = new Mock<IRecommendationRepository>();

            _service = new AnalysisService(
                _mockAuditService.Object,
                _mockRecommendationService.Object,
                _mockAnalysisResultRepository.Object,
                _mockRecommendationRepository.Object
            );
        }

        [Fact]
        public void CreateAnalysisEntity_ShouldReturnValidEntity()
        {
            var websiteId = Guid.NewGuid();
            var analysisType = "SEO";
            var score = 90;
            var resultData = "{\"key\":\"value\"}";
            var summary = "Test summary";

            var result = _service.CreateAnalysisEntity(websiteId, analysisType, score, resultData, summary);

            Assert.NotNull(result);
            Assert.Equal(websiteId, result.WebsiteId);
            Assert.Equal(analysisType, result.AnalysisType);
            Assert.Equal(score, result.Score);
            Assert.Equal(resultData, result.ResultData);
            Assert.Equal(summary, result.Summary);
        }

        [Fact]
        public async Task CreateAnalysisAsync_ShouldPersistAndReturnEntity()
        {
            var websiteId = Guid.NewGuid();
            var analysisType = "Performance";
            var score = 80;
            var resultData = "{\"speed\":\"fast\"}";
            var summary = "Performance summary";

            AnalysisResult? persisted = null;
            _mockAnalysisResultRepository
                .Setup(r => r.AddAsync(It.IsAny<AnalysisResult>()))
                .Callback<AnalysisResult>(a => persisted = a)
                .Returns(Task.FromResult<AnalysisResult?>(persisted)); // Fix: Explicitly cast to Task<AnalysisResult?>  

            var result = await _service.CreateAnalysisAsync(websiteId, analysisType, score, resultData, summary);

            Assert.NotNull(result);
            Assert.Equal(websiteId, result.WebsiteId);
            Assert.Equal(analysisType, result.AnalysisType);
            Assert.Equal(score, result.Score);
            Assert.Equal(resultData, result.ResultData);
            Assert.Equal(summary, result.Summary);
            Assert.Equal(result, persisted);
            _mockAuditService.Verify(a => a.SetCreationAudit(It.IsAny<AnalysisResult>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAnalysisAsync_ShouldUpdateAndPersist()
        {
            var analysis = AnalysisResult.Create(Guid.NewGuid(), "SEO", 50, "{\"a\":1}", "Old summary");
            var newScore = 75;
            var newResultData = "{\"a\":2}";
            var newSummary = "Updated summary";

            _mockAnalysisResultRepository
                .Setup(r => r.UpdateAsync(It.IsAny<AnalysisResult>()))
                .Returns(Task.CompletedTask);

            await _service.UpdateAnalysisAsync(analysis, newScore, newResultData, newSummary);

            Assert.Equal(newScore, analysis.Score);
            Assert.Equal(newResultData, analysis.ResultData);
            Assert.Equal(newSummary, analysis.Summary);
            _mockAuditService.Verify(a => a.UpdateModificationDate(analysis), Times.Once);
            _mockAnalysisResultRepository.Verify(r => r.UpdateAsync(analysis), Times.Once);
        }

        [Fact]
        public async Task ValidateAnalysisDataAsync_ShouldReturnTrueForValidJson()
        {
            var validJson = "{\"foo\":\"bar\"}";
            var result = await _service.ValidateAnalysisDataAsync(validJson);
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateAnalysisDataAsync_ShouldReturnFalseForInvalidJson()
        {
            var invalidJson = "{foo:bar}";
            var result = await _service.ValidateAnalysisDataAsync(invalidJson);
            Assert.False(result);
        }

        [Fact]
        public async Task AnalyzeWebsiteAsync_ShouldCreateAndPersistAnalysis()
        {
            var websiteId = Guid.NewGuid();
            var analysisType = "Accessibility";
            AnalysisResult? persisted = null;
            _mockAnalysisResultRepository
                .Setup(r => r.AddAsync(It.IsAny<AnalysisResult>()))
                .Callback<AnalysisResult>(a => persisted = a)
                .Returns(Task.FromResult<AnalysisResult?>(persisted));

            var result = await _service.AnalyzeWebsiteAsync(websiteId, analysisType);

            Assert.NotNull(result);
            Assert.Equal(websiteId, result.WebsiteId);
            Assert.Equal(analysisType, result.AnalysisType);
            Assert.Equal(80, result.Score);
            Assert.Equal("{\"status\":\"ok\"}", result.ResultData);
            Assert.Equal(result, persisted);
            _mockAuditService.Verify(a => a.SetCreationAudit(It.IsAny<AnalysisResult>()), Times.Once);
        }

        [Fact]
        public async Task GetAnalysisResultByIdAsync_ShouldReturnResult()
        {
            var id = Guid.NewGuid();
            var expected = AnalysisResult.Create(Guid.NewGuid(), "SEO", 90, "{}", "summary");
            _mockAnalysisResultRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(expected);

            var result = await _service.GetAnalysisResultByIdAsync(id);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAnalysisResultsByWebsiteIdAsync_ShouldReturnResults()
        {
            var websiteId = Guid.NewGuid();
            var list = new List<AnalysisResult>
            {
                AnalysisResult.Create(websiteId, "SEO", 90, "{}", "summary"),
                AnalysisResult.Create(websiteId, "Performance", 80, "{}", "summary2")
            };
            _mockAnalysisResultRepository.Setup(r => r.GetByWebsiteIdAsync(websiteId)).ReturnsAsync(list);

            var result = await _service.GetAnalysisResultsByWebsiteIdAsync(websiteId);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetLatestAnalysisResultAsync_ShouldReturnLatest()
        {
            var websiteId = Guid.NewGuid();
            var latest = AnalysisResult.Create(websiteId, "SEO", 90, "{}", "summary");
            _mockAnalysisResultRepository.Setup(r => r.GetLatestByWebsiteIdAsync(websiteId)).ReturnsAsync(latest);

            var result = await _service.GetLatestAnalysisResultAsync(websiteId);

            Assert.Equal(latest, result);
        }

        [Fact]
        public async Task AddRecommendationAsync_ShouldAddAndPersistRecommendation()
        {
            var analysisId = Guid.NewGuid();
            var analysis = AnalysisResult.Create(Guid.NewGuid(), "SEO", 90, "{}", "summary");
            typeof(BaseEntity).GetProperty("Id")?.SetValue(analysis, analysisId);

            var recommendation = Recommendation.Create(analysisId, "Title", "Desc", 1, "SEO");
            _mockAnalysisResultRepository.Setup(r => r.GetByIdAsync(analysisId)).ReturnsAsync(analysis);
            _mockRecommendationService.Setup(s => s.CreateRecommendationAsync(analysisId, "Title", "Desc", 1, "SEO")).ReturnsAsync(recommendation);
            _mockRecommendationRepository.Setup(r => r.AddAsync(recommendation)).Returns(Task.FromResult(recommendation)); // Fix: Ensure the return type matches Task<Recommendation>  
            _mockAnalysisResultRepository.Setup(r => r.UpdateAsync(analysis)).Returns(Task.CompletedTask);

            var result = await _service.AddRecommendationAsync(analysisId, "Title", "Desc", 1, "SEO");

            Assert.Contains(recommendation, analysis.Recommendations);
            Assert.Equal(recommendation, result);
            _mockAuditService.Verify(a => a.UpdateModificationDate(analysis), Times.Once);
            _mockAnalysisResultRepository.Verify(r => r.UpdateAsync(analysis), Times.Once);
            _mockRecommendationRepository.Verify(r => r.AddAsync(recommendation), Times.Once);
        }

        [Fact]
        public async Task GetRecommendationsByAnalysisResultIdAsync_ShouldReturnRecommendations()
        {
            var analysisId = Guid.NewGuid();
            var recs = new List<Recommendation>
            {
                Recommendation.Create(analysisId, "Title1", "Desc1", 1, "SEO"),
                Recommendation.Create(analysisId, "Title2", "Desc2", 2, "SEO")
            };
            _mockRecommendationRepository.Setup(r => r.GetByAnalysisResultIdAsync(analysisId)).ReturnsAsync(recs);

            var result = await _service.GetRecommendationsByAnalysisResultIdAsync(analysisId);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task MarkRecommendationAsImplementedAsync_ShouldUpdateStatus()
        {
            var recId = Guid.NewGuid();
            var rec = Recommendation.Create(Guid.NewGuid(), "Title", "Desc", 1, "SEO");
            _mockRecommendationRepository.Setup(r => r.GetByIdAsync(recId)).ReturnsAsync(rec);
            _mockRecommendationRepository.Setup(r => r.UpdateAsync(rec)).Returns(Task.CompletedTask);

            await _service.MarkRecommendationAsImplementedAsync(recId);

            _mockRecommendationRepository.Verify(r => r.UpdateAsync(rec), Times.Once);
            Assert.Equal(RecommendationStatus.Implemented, rec.Status);
        }

        [Fact]
        public async Task MarkRecommendationAsNotImplementedAsync_ShouldUpdateStatus()
        {
            var recId = Guid.NewGuid();
            var rec = Recommendation.Create(Guid.NewGuid(), "Title", "Desc", 1, "SEO");
            _mockRecommendationRepository.Setup(r => r.GetByIdAsync(recId)).ReturnsAsync(rec);
            _mockRecommendationRepository.Setup(r => r.UpdateAsync(rec)).Returns(Task.CompletedTask);

            await _service.MarkRecommendationAsNotImplementedAsync(recId);

            _mockRecommendationRepository.Verify(r => r.UpdateAsync(rec), Times.Once);
            Assert.Equal(RecommendationStatus.Pending, rec.Status);
        }

        [Theory]
        [InlineData("", 50, "{\"a\":1}")]
        [InlineData("SEO", -1, "{\"a\":1}")]
        [InlineData("SEO", 50, "{invalid}")]
        public async Task CreateAnalysisAsync_InvalidInput_ShouldThrow(string analysisType, int score, string resultData)
        {
            var websiteId = Guid.NewGuid();
            var summary = "summary";
            await Assert.ThrowsAnyAsync<ArgumentException>(() =>
                _service.CreateAnalysisAsync(websiteId, analysisType, score, resultData, summary));
        }

        [Fact]
        public async Task AddRecommendationAsync_InvalidAnalysisId_ShouldThrow()
        {
            _mockAnalysisResultRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((AnalysisResult)null!);
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.AddRecommendationAsync(Guid.NewGuid(), "Title", "Desc", 1, "SEO"));
        }

        [Fact]
        public async Task MarkRecommendationAsImplementedAsync_InvalidId_ShouldThrow()
        {
            _mockRecommendationRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Recommendation)null!);
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.MarkRecommendationAsImplementedAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task MarkRecommendationAsNotImplementedAsync_InvalidId_ShouldThrow()
        {
            _mockRecommendationRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Recommendation)null!);
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.MarkRecommendationAsNotImplementedAsync(Guid.NewGuid()));
        }
    }
}