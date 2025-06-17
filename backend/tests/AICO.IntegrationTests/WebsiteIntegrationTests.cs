using AICO.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using AICO.Application.Interfaces.Services; // Added this line

namespace AICO.IntegrationTests
{
    public class WebsiteIntegrationTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly IServiceScope _scope;
        private readonly ISnippetService _snippetService;
        private readonly IVariantService _variantService;

        public WebsiteIntegrationTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _snippetService = _scope.ServiceProvider.GetRequiredService<ISnippetService>();
            _variantService = _scope.ServiceProvider.GetRequiredService<IVariantService>();
        }

        [Fact]
        public async Task GenerateJavaScriptSnippet_WithActiveAbTest_ShouldIncludeVariantLogic()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var abTest = await CreateActiveAbTestAsync(websiteId);

            // Act
            var snippet = await _snippetService.GenerateTrackingSnippetAsync(websiteId);

            // Assert
            Assert.NotNull(snippet);
            Assert.Contains("AICO", snippet.Content);
            Assert.Contains("trackConversion", snippet.Content);
            Assert.Contains(abTest.Id.ToString(), snippet.Content);
            Assert.Contains("variant", snippet.Content.ToLower());
        }

        [Fact]
        public async Task ServeVariant_WithValidUserId_ShouldReturnConsistentVariant()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var userId = "test-user-123";
            var abTest = await CreateActiveAbTestAsync(websiteId);

            // Act
            var firstServe = await _variantService.ServeVariantAsync(userId, abTest.Id);
            var secondServe = await _variantService.ServeVariantAsync(userId, abTest.Id);

            // Assert
            Assert.NotNull(firstServe);
            Assert.NotNull(secondServe);
            Assert.Equal(firstServe.Id, secondServe.Id);
        }

        [Fact]
        public async Task EndToEndIntegration_CreateTestServeVariantTrackConversion_ShouldWorkCorrectly()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var userId = "integration-test-user";

            // Act & Assert
            // 1. Create A/B Test
            var abTest = await CreateActiveAbTestAsync(websiteId);
            Assert.NotNull(abTest);

            // 2. Serve Variant
            var variant = await _variantService.ServeVariantAsync(userId, abTest.Id);
            Assert.NotNull(variant);

            // 3. Track Conversion
            var conversionTracked = await _variantService.TrackConversionAsync(userId, variant.Id, 99.99m);
            Assert.True(conversionTracked);

            // 4. Verify Data
            var stats = await _variantService.GetVariantStatsAsync(variant.Id);
            Assert.True(stats.Conversions > 0);
            Assert.True(stats.Revenue > 0);
        }

        private async Task<AbTest> CreateActiveAbTestAsync(Guid websiteId)
        {
            var campaignId = Guid.NewGuid();
            var abTest = AbTest.Create(
                campaignId,
                "Integration Test",
                "Test Description",
                TestType.Create("Button Color"),
                50,
                "conversion_rate",
                DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow.AddDays(30)
            );

            // Start the test
            abTest.Start();
            return abTest;
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}