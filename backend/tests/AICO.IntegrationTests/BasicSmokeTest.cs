using Microsoft.Extensions.DependencyInjection;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AICO.Infrastructure.DependencyInjection;
using AICO.Domain.Entities;
using AICO.Domain.ValueObjects;
using AICO.Domain.Interfaces.Services;
using Xunit;
using FluentAssertions;

namespace AICO.IntegrationTests
{
    public class BasicSmokeTest
    {
        [Fact]
        public void ServiceRegistration_ShouldWork()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // Add Entity Framework with in-memory database
            services.AddDbContext<AicoDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
            
            // Add all services
            services.AddDomainServices();
            services.AddRepositories();
            services.AddApplicationServices();
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Act & Assert
            var campaignService = serviceProvider.GetService<ICampaignService>();
            var abTestService = serviceProvider.GetService<IAbTestService>();
            
            campaignService.Should().NotBeNull();
            abTestService.Should().NotBeNull();
        }
        
        [Fact]
        public void Campaign_Creation_ShouldWork()
        {
            // Arrange
            var services = new ServiceCollection();
            
            services.AddDbContext<AicoDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb2");
            });
            
            services.AddDomainServices();
            services.AddRepositories();
            services.AddApplicationServices();
            
            var serviceProvider = services.BuildServiceProvider();
            var campaignService = serviceProvider.GetRequiredService<ICampaignService>();
            
            // Act
            var campaign = new Campaign(
                "Test Campaign",
                "Test Description",
                Guid.NewGuid(), // websiteId
                Guid.NewGuid()  // userId
            );
            
            // Assert
            campaign.Should().NotBeNull();
            campaign.Name.Should().Be("Test Campaign");
            campaign.Description.Should().Be("Test Description");
        }
        
        [Fact]
        public void AbTest_Creation_ShouldWork()
        {
            // Arrange & Act
            var abTest = AbTest.Create(
                "Test A/B Test",
                "Test Description",
                Guid.NewGuid(), // campaignId
                TestType.Headline,
                "h1", // targetSelector
                "Original Content", // originalContent
                "conversion_rate" // primaryMetric
            );
            
            // Assert
            abTest.Should().NotBeNull();
            abTest.Name.Should().Be("Test A/B Test");
            abTest.TestType.Should().Be(TestType.Headline);
        }
    }
}