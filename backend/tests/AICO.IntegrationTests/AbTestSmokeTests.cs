using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AICO.IntegrationTests
{
    public class AbTestSmokeTests : IClassFixture<TestWebApplicationFactory>
    { 
        private readonly TestWebApplicationFactory _factory;

        public AbTestSmokeTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateAbTest_SmokeTest()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var abTestService = scope.ServiceProvider.GetRequiredService<IAbTestService>();
            var campaignId = Guid.NewGuid();

            var variants = new List<AbTestVariant>
            {
                new AbTestVariant { Name = "Control", Content = "Original", IsControl = true, TrafficSplitPercentage = 50 },
                new AbTestVariant { Name = "Variant A", Content = "New Version", IsControl = false, TrafficSplitPercentage = 50 }
            };

            // Act
            var result = await abTestService.CreateTestAsync(campaignId, variants);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Variants.Count);
        }

        [Fact]
        public async Task GetVariantForVisitor_SmokeTest()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var abTestService = scope.ServiceProvider.GetRequiredService<IAbTestService>();
            var campaignId = Guid.NewGuid();

            var variants = new List<AbTestVariant>
            {
                new AbTestVariant { Name = "Control", Content = "Original", IsControl = true, TrafficSplitPercentage = 50 },
                new AbTestVariant { Name = "Variant A", Content = "New Version", IsControl = false, TrafficSplitPercentage = 50 }
            };

            var test = await abTestService.CreateTestAsync(campaignId, variants);
            var visitorId = Guid.NewGuid().ToString();

            // Act
            var variant = await abTestService.GetVariantForVisitorAsync(test.Id, visitorId);

            // Assert
            Assert.NotNull(variant);
        }

        [Fact]
        public async Task RecordConversion_SmokeTest()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var abTestService = scope.ServiceProvider.GetRequiredService<IAbTestService>();
            var campaignId = Guid.NewGuid();

            var variants = new List<AbTestVariant>
            {
                new AbTestVariant { Name = "Control", Content = "Original", IsControl = true, TrafficSplitPercentage = 50 },
                new AbTestVariant { Name = "Variant A", Content = "New Version", IsControl = false, TrafficSplitPercentage = 50 }
            };

            var test = await abTestService.CreateTestAsync(campaignId, variants);
            var visitorId = Guid.NewGuid().ToString();
            var variant = await abTestService.GetVariantForVisitorAsync(test.Id, visitorId);

            // Act
            await abTestService.RecordVariantConversionAsync(variant.Id, visitorId);

            // Assert
            // No direct assertion, but if no exception is thrown, the test passes.
        }
    }
}
