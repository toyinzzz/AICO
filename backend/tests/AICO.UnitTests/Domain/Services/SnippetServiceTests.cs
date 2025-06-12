using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace AICO.UnitTests.Domain.Services
{
    public class SnippetServiceTests
    {
        private readonly Mock<ISnippetRepository> _mockSnippetRepository;
        private readonly Mock<IWebsiteRepository> _mockWebsiteRepository;
        private readonly SnippetService _snippetService;

        public SnippetServiceTests()
        {
            _mockSnippetRepository = new Mock<ISnippetRepository>();
            _mockWebsiteRepository = new Mock<IWebsiteRepository>();
            _snippetService = new SnippetService(
                _mockSnippetRepository.Object,
                _mockWebsiteRepository.Object);
        }

        [Fact]
        public async Task GenerateSnippetAsync_WithValidWebsite_ShouldReturnJavaScriptCode()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var website = new Website("https://example.com", "Example Site");

            _mockWebsiteRepository.Setup(x => x.GetByIdAsync(websiteId))
                .ReturnsAsync(website);

            // Act
            var result = await _snippetService.GenerateSnippetAsync(websiteId);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("<script", result);
            Assert.Contains(websiteId.ToString(), result);
            Assert.Contains("AICO", result); // Should contain our tracking code
        }

        [Fact]
        public async Task ValidateSnippetInstallationAsync_WithInstalledSnippet_ShouldReturnTrue()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var website = new Website("https://example.com", "Example Site");

            _mockWebsiteRepository.Setup(x => x.GetByIdAsync(websiteId))
                .ReturnsAsync(website);

            // Act
            var result = await _snippetService.ValidateSnippetInstallationAsync(websiteId);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("/product/*", "/product/123", true)]
        [InlineData("/product/*", "/category/123", false)]
        [InlineData("/checkout", "/checkout", true)]
        [InlineData("/checkout", "/checkout/step2", false)]
        public void ShouldTargetPage_WithVariousPatterns_ShouldReturnCorrectResult(
            string targetPattern, string currentPage, bool expectedResult)
        {
            // Arrange & Act
            var result = _snippetService.ShouldTargetPage(targetPattern, currentPage);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GetVariantForPageAsync_WithActiveTest_ShouldReturnCorrectVariant()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var pageUrl = "/product/123";
            var visitorId = "visitor_123";

            var variant = new Variant(Guid.NewGuid(), "Test Variant", "<html>Test content</html>");

            _mockSnippetRepository.Setup(x => x.GetActiveVariantForPageAsync(websiteId, pageUrl, visitorId))
                .ReturnsAsync(variant);

            // Act
            var result = await _snippetService.GetVariantForPageAsync(websiteId, pageUrl, visitorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(variant.Id, result.Id);
        }

        [Fact]
        public async Task GetSnippetAnalyticsAsync_WithValidWebsite_ShouldReturnAnalytics()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var expectedAnalytics = new SnippetAnalytics
            {
                TotalPageViews = 10000,
                UniqueVisitors = 2500,
                TestsRunning = 3,
                ConversionRate = 0.045,
                LastActivity = DateTime.UtcNow.AddMinutes(-5)
            };

            _mockSnippetRepository.Setup(x => x.GetAnalyticsAsync(websiteId))
                .ReturnsAsync(expectedAnalytics);

            // Act
            var result = await _snippetService.GetSnippetAnalyticsAsync(websiteId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAnalytics.TotalPageViews, result.TotalPageViews);
            Assert.Equal(expectedAnalytics.UniqueVisitors, result.UniqueVisitors);
            Assert.Equal(expectedAnalytics.TestsRunning, result.TestsRunning);
        }

        [Fact]
        public async Task ServeVariantAsync_WithValidRequest_ShouldReturnVariantContent()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var pageUrl = "/landing";
            var visitorId = "visitor_123";
            var userAgent = "Mozilla/5.0...";

            var variant = new Variant(Guid.NewGuid(), "Test Variant", "<html><body>Modified content</body></html>");

            _mockSnippetRepository.Setup(x => x.GetActiveVariantForPageAsync(websiteId, pageUrl, visitorId))
                .ReturnsAsync(variant);

            // Act
            var result = await _snippetService.ServeVariantAsync(websiteId, pageUrl, visitorId, userAgent);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Modified content", result.HtmlContent);
        }
    }
}