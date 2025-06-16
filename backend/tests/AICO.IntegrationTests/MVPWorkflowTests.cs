using AICO.Application.Interfaces.Commands;
using AICO.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AICO.IntegrationTests
{
    public class MVPWorkflowTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly IServiceScope _scope;

        public MVPWorkflowTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
        }

        [Fact]
        public async Task CompleteABTestWorkflow_ShouldExecuteSuccessfully()
        {
            // Arrange
            var campaignHandler = _scope.ServiceProvider.GetRequiredService<ICampaignCommandHandler>();
            var abTestHandler = _scope.ServiceProvider.GetRequiredService<IAbTestCommandHandler>();
            var variantGenerator = _scope.ServiceProvider.GetRequiredService<IVariantGenerationService>();
            var revenueService = _scope.ServiceProvider.GetRequiredService<IRevenueTrackingService>();
            var abTestService = _scope.ServiceProvider.GetRequiredService<IAbTestService>();

            // Act & Assert - Complete workflow

            // 1. Create Campaign
            var createCampaignCommand = new CreateCampaignCommand(
                "Test Campaign",
                "Integration test campaign",
                DateTime.UtcNow.AddDays(30)
            );
            var campaign = await campaignHandler.HandleAsync(createCampaignCommand);
            Assert.NotNull(campaign);

            // 2. Generate AI Variants
            var variants = await variantGenerator.GenerateVariantsAsync(
                "https://example.com/landing",
                new VariantGenerationRequest
                {
                    TargetAudience = "Tech professionals",
                    OptimizationGoal = "Increase conversions",
                    VariantCount = 2
                });
            Assert.Equal(2, variants.Count);

            // 3. Create A/B Test
            var createTestCommand = new CreateAbTestCommand(
                campaign.Id,
                "Landing Page Test",
                variants.Select(v => v.Id).ToList(),
                50 // 50/50 split
            );
            var abTest = await abTestHandler.HandleAsync(createTestCommand);
            Assert.NotNull(abTest);

            // 4. Start Test
            var startTestCommand = new StartAbTestCommand(abTest.Id);
            await abTestHandler.HandleAsync(startTestCommand);

            // 5. Simulate Traffic and Conversions
            for (int i = 0; i < 1000; i++)
            {
                var visitorId = $"visitor_{i}";

                // Get variant assignment
                var assignedVariant = await abTestService.GetVariantForVisitorAsync(abTest.Id, visitorId);

                // Record view
                await abTestService.RecordVariantViewAsync(assignedVariant.Id, visitorId);

                // Simulate conversion (20% conversion rate)
                if (i % 5 == 0)
                {
                    await abTestService.RecordVariantConversionAsync(assignedVariant.Id, visitorId, 99.99m, "purchase");
                    await revenueService.RecordRevenueEventAsync(campaign.Id, assignedVariant.Id, 99.99m, "USD", $"txn_{i}");
                }
            }

            // 6. Check Statistical Significance
            var isSignificant = await abTestService.IsTestStatisticallySignificantAsync(abTest.Id, 0.95);

            // 7. Get Test Results
            var testResults = await abTestService.GetTestResultsAsync(abTest.Id);
            Assert.NotNull(testResults);
            Assert.True(testResults.ControlVariant.Views > 0);
            Assert.True(testResults.TestVariant.Views > 0);

            // 8. Calculate Profit Metrics
            var profitLift = await revenueService.CalculateProfitLiftAsync(campaign.Id);
            var roi = await revenueService.CalculateROIAsync(campaign.Id);

            Assert.True(profitLift != 0);
            Assert.True(roi > 0);

            // 9. If significant, declare winner
            if (isSignificant)
            {
                var winner = await abTestService.DeclareWinnerAsync(abTest.Id);
                Assert.NotNull(winner);
            }
        }

        [Fact]
        public async Task WebsiteIntegrationWorkflow_ShouldServeVariantsCorrectly()
        {
            // Arrange
            var snippetService = _scope.ServiceProvider.GetRequiredService<ISnippetService>();
            var websiteId = Guid.NewGuid();

            // Act & Assert

            // 1. Generate snippet
            var snippet = await snippetService.GenerateSnippetAsync(websiteId);
            Assert.NotNull(snippet);
            Assert.Contains("<script", snippet);

            // 2. Validate installation
            var isInstalled = await snippetService.ValidateSnippetInstallationAsync(websiteId);
            Assert.True(isInstalled);

            // 3. Test page targeting
            Assert.True(snippetService.ShouldTargetPage("/product/*", "/product/123"));
            Assert.False(snippetService.ShouldTargetPage("/product/*", "/category/456"));

            // 4. Get analytics
            var analytics = await snippetService.GetSnippetAnalyticsAsync(websiteId);
            Assert.NotNull(analytics);
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}