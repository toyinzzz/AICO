using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using AICO.Domain.DTOs;

namespace AICO.IntegrationTests
{
    public class MVPWorkflowTests : IClassFixture<TestWebApplicationFactory>, IDisposable
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly IServiceScope _scope;
        private readonly ICampaignService _campaignService;
        private readonly IAbTestService _abTestService;
        private readonly IVariantGenerationService _variantGenerationService;
        private readonly IRevenueTrackingService _revenueService;
        private readonly ISnippetService _snippetService;

        public MVPWorkflowTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _campaignService = _scope.ServiceProvider.GetRequiredService<ICampaignService>();
            _abTestService = _scope.ServiceProvider.GetRequiredService<IAbTestService>();
            _variantGenerationService = _scope.ServiceProvider.GetRequiredService<IVariantGenerationService>();
            _revenueService = _scope.ServiceProvider.GetRequiredService<IRevenueTrackingService>();
            _snippetService = _scope.ServiceProvider.GetRequiredService<ISnippetService>();
        }

        [Fact]
        public async Task CompleteABTestWorkflow_ShouldExecuteSuccessfully()
        {
            // Arrange
            var campaign = await _campaignService.CreateCampaignAsync(Guid.NewGuid(), Guid.NewGuid(), "Test Campaign", "https://example.com", "Integration test campaign");

            // Convert List<Variant> to List<AbTestVariant>
            var variants = await _variantGenerationService.GenerateVariantsAsync("https://example.com/landing", new VariantGenerationRequest { TargetAudience = "Tech professionals", VariantCount = 2 });
            var abTestVariants = variants.Select(v => new AbTestVariant(
                campaign.Id, // abTestId
                v.Name,
                v.Content,
                v.TrafficAllocation,
                null, // description
                v.IsControl
            )).ToList();

            var abTest = await _abTestService.CreateTestAsync(campaign.Id, abTestVariants);

            var visitorId = Guid.NewGuid().ToString();

            // Act
            await _abTestService.RecordVariantViewAsync(abTestVariants[0].Id, visitorId);
            await _abTestService.RecordVariantConversionAsync(abTestVariants[0].Id, visitorId, 100.0m);
            await _revenueService.RecordRevenueEventAsync(campaign.Id, abTestVariants[0].Id, 100.0m, "USD");

            // Assert
            var testResults = await _abTestService.GetTestResultsAsync(abTest.Id);
            Assert.NotNull(testResults);
            Assert.NotEmpty(testResults.TestVariants);

            var revenueMetrics = await _revenueService.GetRevenueMetricsAsync(campaign.Id);
            Assert.NotNull(revenueMetrics);
            Assert.True(revenueMetrics.TotalRevenue > 0);

            var profitLift = await _revenueService.CalculateProfitLiftAsync(campaign.Id);
            Assert.True(profitLift > 0);

            var userAssignedVariant = await _abTestService.GetVariantForVisitorAsync(abTest.Id, visitorId);
            Assert.NotNull(userAssignedVariant);
            Assert.Equal(abTestVariants[0].Id, userAssignedVariant.Id);
        }

        [Fact]
        public async Task WebsiteIntegrationWorkflow_ShouldServeVariantsCorrectly()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var campaign = await _campaignService.CreateCampaignAsync(websiteId, Guid.NewGuid(), "Website Integration Campaign", "https://example.com", "Test campaign for website integration");
            var variants = await _variantGenerationService.GenerateVariantsAsync("https://example.com/landing", new VariantGenerationRequest { TargetAudience = "Tech professionals", VariantCount = 2 });
            var abTestVariants = variants.Select(v => new AbTestVariant(
                campaign.Id, // abTestId
                v.Name,
                v.Content,
                v.TrafficAllocation,
                null, // description
                v.IsControl
            )).ToList();
            var abTest = await _abTestService.CreateTestAsync(campaign.Id, abTestVariants);
            var visitorId = Guid.NewGuid().ToString();

            // Act
            var snippet = await _snippetService.GenerateSnippetAsync(websiteId);
            var isInstalled = await _snippetService.ValidateSnippetInstallationAsync(websiteId);
            var userAssignedVariant = await _abTestService.GetVariantForVisitorAsync(abTest.Id, visitorId);

            // Assert
            Assert.NotNull(snippet);
            Assert.Contains("<script", snippet);
            Assert.True(isInstalled);
            Assert.NotNull(userAssignedVariant);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _scope?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}