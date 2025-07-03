using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;
using FluentAssertions;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static AICO.Domain.Entities.Campaign;

namespace AICO.IntegrationTests
{
    /// <summary>
    /// Comprehensive MVP integration tests covering all critical production scenarios
    /// </summary>
    public class ComprehensiveMVPTests : IClassFixture<TestWebApplicationFactory>, IDisposable
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly AicoDbContext _context;
        private readonly ICampaignService _campaignService;
        private readonly IAbTestService _abTestService;
        private readonly IVariantGenerationService _variantService;
        private readonly IConversionService _conversionService;
        private readonly ISnippetService _snippetService;
        private readonly IStatisticalAnalysisService _analysisService;

        public ComprehensiveMVPTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<AicoDbContext>();
            _campaignService = _scope.ServiceProvider.GetRequiredService<ICampaignService>();
            _abTestService = _scope.ServiceProvider.GetRequiredService<IAbTestService>();
            _variantService = _scope.ServiceProvider.GetRequiredService<IVariantGenerationService>();
            _conversionService = _scope.ServiceProvider.GetRequiredService<IConversionService>();
            _snippetService = _scope.ServiceProvider.GetRequiredService<ISnippetService>();
            _analysisService = _scope.ServiceProvider.GetRequiredService<IStatisticalAnalysisService>();
        }

        #region Campaign Management Tests

        [Fact]
        public async Task CreateCampaign_WithValidData_ShouldSucceed()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var websiteId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var campaignName = "Test Campaign " + Guid.NewGuid();

            // Act
            var campaign = await _campaignService.CreateCampaignAsync(
                campaignName, 
                "Test Description", 
                websiteId, 
                userId);

            // Assert
            campaign.Should().NotBeNull();
            campaign.Name.Should().Be(campaignName);
            campaign.Status.Should().Be(CampaignStatus.Draft);
            campaign.WebsiteId.Should().Be(websiteId);
            campaign.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task UpdateCampaign_WithValidData_ShouldSucceed()
        {
            // Arrange
            var campaignId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var newName = "Updated Campaign Name";
            var newDescription = "Updated Description";

            // Act
            await _campaignService.UpdateCampaignAsync(campaignId, newName, newDescription);
            var updatedCampaign = await _campaignService.GetCampaignByIdAsync(campaignId);

            // Assert
            updatedCampaign.Should().NotBeNull();
            updatedCampaign.Name.Should().Be(newName);
            updatedCampaign.Description.Should().Be(newDescription);
        }

        [Fact]
        public async Task StartCampaign_WithValidCampaign_ShouldChangeStatus()
        {
            // Arrange
            var campaignId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Act
            await _campaignService.StartCampaignAsync(campaignId);
            var campaign = await _campaignService.GetCampaignByIdAsync(campaignId);

            // Assert
            campaign.Should().NotBeNull();
            campaign.Status.Should().Be(CampaignStatus.Active);
            campaign.StartDate.Should().NotBeNull();
        }

        #endregion

        #region A/B Test Management Tests

        [Fact]
        public async Task CreateAbTest_WithValidData_ShouldSucceed()
        {
            // Arrange
            var campaignId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var websiteId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var testName = "Test AB Test " + Guid.NewGuid();

            // Act
            var abTest = await _abTestService.CreateAbTestAsync(
                testName,
                "Test Description",
                campaignId,
                websiteId,
                ".test-element",
                "Original Content",
                "conversion_rate");

            // Assert
            abTest.Should().NotBeNull();
            abTest.Name.Should().Be(testName);
            abTest.Status.Should().Be(AbTestStatus.Draft);
            abTest.CampaignId.Should().Be(campaignId);
            abTest.WebsiteId.Should().Be(websiteId);
        }

        [Fact]
        public async Task StartAbTest_WithValidTest_ShouldChangeStatus()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            // Act
            await _abTestService.StartAbTestAsync(abTestId);
            var abTest = await _abTestService.GetAbTestByIdAsync(abTestId);

            // Assert
            abTest.Should().NotBeNull();
            abTest.Status.Should().Be(AbTestStatus.Running);
            abTest.StartedAt.Should().NotBeNull();
        }

        [Fact]
        public async Task StopAbTest_WithRunningTest_ShouldChangeStatus()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            await _abTestService.StartAbTestAsync(abTestId);

            // Act
            await _abTestService.StopAbTestAsync(abTestId);
            var abTest = await _abTestService.GetAbTestByIdAsync(abTestId);

            // Assert
            abTest.Should().NotBeNull();
            abTest.Status.Should().Be(AbTestStatus.Stopped);
            abTest.EndedAt.Should().NotBeNull();
        }

        #endregion

        #region Variant Management Tests

        [Fact]
        public async Task GetVariantForVisitor_WithValidTest_ShouldReturnConsistentVariant()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var visitorId = "test-visitor-123";
            await _abTestService.StartAbTestAsync(abTestId);

            // Act
            var variant1 = await _variantService.GetVariantForVisitorAsync(abTestId, visitorId);
            var variant2 = await _variantService.GetVariantForVisitorAsync(abTestId, visitorId);

            // Assert
            variant1.Should().NotBeNull();
            variant2.Should().NotBeNull();
            variant1.Id.Should().Be(variant2.Id); // Should be consistent
        }

        [Fact]
        public async Task GetVariantForVisitor_WithMultipleVisitors_ShouldDistributeTraffic()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            await _abTestService.StartAbTestAsync(abTestId);
            var variants = new List<Guid>();

            // Act - Get variants for 100 different visitors
            for (int i = 0; i < 100; i++)
            {
                var variant = await _variantService.GetVariantForVisitorAsync(abTestId, $"visitor-{i}");
                variants.Add(variant.Id);
            }

            // Assert - Should have both variants represented
            var uniqueVariants = variants.Distinct().ToList();
            uniqueVariants.Should().HaveCountGreaterThan(1);
        }

        #endregion

        #region Conversion Tracking Tests

        [Fact]
        public async Task RecordConversion_WithValidData_ShouldSucceed()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var visitorId = "conversion-test-visitor";
            await _abTestService.StartAbTestAsync(abTestId);
            var variant = await _variantService.GetVariantForVisitorAsync(abTestId, visitorId);

            // Act
            await _conversionService.RecordConversionAsync(
                variant.Id,
                visitorId,
                100.0m,
                "purchase");

            // Assert
            var conversions = await _context.Conversions
                .Where(c => c.SessionId.ToString() == visitorId)
                .ToListAsync();
            
            conversions.Should().HaveCount(1);
            conversions.First().Value.Should().Be(100.0m);
        }

        [Fact]
        public async Task RecordMultipleConversions_ShouldUpdateVariantMetrics()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            await _abTestService.StartAbTestAsync(abTestId);
            
            // Act - Record multiple conversions
            for (int i = 0; i < 10; i++)
            {
                var visitorId = $"metrics-visitor-{i}";
                var variant = await _variantService.GetVariantForVisitorAsync(abTestId, visitorId);
                await _conversionService.RecordConversionAsync(variant.Id, visitorId, 50.0m);
            }

            // Assert
            var abTest = await _abTestService.GetAbTestByIdAsync(abTestId);
            var totalConversions = abTest.Variants.Sum(v => v.Conversions);
            totalConversions.Should().BeGreaterThan(0);
        }

        #endregion

        #region JavaScript Snippet Tests

        [Fact]
        public async Task GenerateSnippet_WithValidWebsite_ShouldReturnValidJavaScript()
        {
            // Arrange
            var websiteId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            // Act
            var snippet = await _snippetService.GenerateSnippetAsync(websiteId);

            // Assert
            snippet.Should().NotBeNullOrEmpty();
            snippet.Should().Contain("script");
            snippet.Should().Contain(websiteId.ToString());
        }

        #endregion

        #region Statistical Analysis Tests

        [Fact]
        public async Task AnalyzeTestResults_WithSufficientData_ShouldProvideStatistics()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            await _abTestService.StartAbTestAsync(abTestId);
            
            // Simulate test data
            await SimulateTestTraffic(abTestId, 1000);

            // Act
            var results = await _analysisService.AnalyzeTestResultsAsync(abTestId);

            // Assert
            results.Should().NotBeNull();
            results.TotalVisitors.Should().BeGreaterThan(0);
            results.TotalConversions.Should().BeGreaterThan(0);
        }

        #endregion

        #region End-to-End Workflow Tests

        [Fact]
        public async Task CompleteAbTestWorkflow_ShouldExecuteSuccessfully()
        {
            // Arrange
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var websiteId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            // Act & Assert - Complete workflow
            
            // 1. Create Campaign
            var campaign = await _campaignService.CreateCampaignAsync(
                "E2E Test Campaign", 
                "End-to-end test", 
                websiteId, 
                userId);
            campaign.Should().NotBeNull();

            // 2. Create A/B Test
            var abTest = await _abTestService.CreateAbTestAsync(
                "E2E A/B Test",
                "End-to-end A/B test",
                campaign.Id,
                websiteId,
                ".e2e-button",
                "Original Button",
                "conversion_rate");
            abTest.Should().NotBeNull();

            // 3. Start Campaign and Test
            await _campaignService.StartCampaignAsync(campaign.Id);
            await _abTestService.StartAbTestAsync(abTest.Id);

            // 4. Simulate Traffic and Conversions
            await SimulateTestTraffic(abTest.Id, 100);

            // 5. Analyze Results
            var results = await _analysisService.AnalyzeTestResultsAsync(abTest.Id);
            results.Should().NotBeNull();
            results.TotalVisitors.Should().BeGreaterThan(0);

            // 6. Stop Test
            await _abTestService.StopAbTestAsync(abTest.Id);
            var stoppedTest = await _abTestService.GetAbTestByIdAsync(abTest.Id);
            stoppedTest.Status.Should().Be(AbTestStatus.Stopped);
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public async Task CreateCampaign_WithInvalidData_ShouldThrowException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _campaignService.CreateCampaignAsync(null, "Description", Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public async Task StartAbTest_WithNonExistentTest_ShouldThrowException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _abTestService.StartAbTestAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetVariant_ForStoppedTest_ShouldThrowException()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            await _abTestService.StartAbTestAsync(abTestId);
            await _abTestService.StopAbTestAsync(abTestId);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _variantService.GetVariantForVisitorAsync(abTestId, "test-visitor"));
        }

        #endregion

        #region Performance Tests

        [Fact]
        public async Task GetVariantForVisitor_WithHighConcurrency_ShouldPerformWell()
        {
            // Arrange
            var abTestId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            await _abTestService.StartAbTestAsync(abTestId);
            var tasks = new List<Task>();

            // Act - Simulate 50 concurrent requests
            for (int i = 0; i < 50; i++)
            {
                var visitorId = $"concurrent-visitor-{i}";
                tasks.Add(_variantService.GetVariantForVisitorAsync(abTestId, visitorId));
            }

            var startTime = DateTime.UtcNow;
            await Task.WhenAll(tasks);
            var duration = DateTime.UtcNow - startTime;

            // Assert - Should complete within reasonable time
            duration.Should().BeLessThan(TimeSpan.FromSeconds(10));
        }

        #endregion

        #region Helper Methods

        private async Task SimulateTestTraffic(Guid abTestId, int visitorCount)
        {
            var random = new Random();
            
            for (int i = 0; i < visitorCount; i++)
            {
                var visitorId = $"sim-visitor-{i}";
                var variant = await _variantService.GetVariantForVisitorAsync(abTestId, visitorId);
                
                // Simulate 20% conversion rate
                if (random.NextDouble() < 0.2)
                {
                    await _conversionService.RecordConversionAsync(
                        variant.Id, 
                        visitorId, 
                        random.Next(10, 200));
                }
            }
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _scope?.Dispose();
                _client?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}