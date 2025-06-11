using System;
using AICO.Domain.Entities;
using AICO.Domain.ValueObjects;
using Xunit;

namespace AICO.Domain.Tests.Entities
{
    public class CampaignTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnCampaign()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var name = "Test Campaign";
            var description = "Test Description";
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddDays(30);
            var budget = 1000m;
            var targetUrl = "https://example.com";

            // Act
            var campaign = Campaign.Create(websiteId, name, description, startDate, endDate, budget, targetUrl);

            // Assert
            Assert.NotNull(campaign);
            Assert.Equal(websiteId, campaign.WebsiteId);
            Assert.Equal(name, campaign.Name);
            Assert.Equal(description, campaign.Description);
            Assert.Equal(startDate, campaign.StartDate);
            Assert.Equal(endDate, campaign.EndDate);
            Assert.Equal(budget, campaign.Budget);
            Assert.Equal(targetUrl, campaign.TargetUrl);
            Assert.Equal(CampaignStatus.Draft, campaign.Status);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddDays(30);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Campaign.Create(websiteId, invalidName, "Description", startDate, endDate, 1000m, "https://example.com"));
        }

        [Fact]
        public void Create_WithNegativeBudget_ShouldThrowArgumentException()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddDays(30);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Campaign.Create(websiteId, "Test", "Description", startDate, endDate, -100m, "https://example.com"));
        }

        [Fact]
        public void Create_WithEndDateBeforeStartDate_ShouldThrowArgumentException()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(30);
            var endDate = DateTime.UtcNow.AddDays(1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Campaign.Create(websiteId, "Test", "Description", startDate, endDate, 1000m, "https://example.com"));
        }

        [Fact]
        public void UpdateStatus_WithValidTransition_ShouldUpdateStatus()
        {
            // Arrange
            var campaign = CreateValidCampaign();
            var mockValidator = new Mock<ICampaignStateValidationService>();
            mockValidator.Setup(x => x.CanTransitionTo(CampaignStatus.Draft, CampaignStatus.Active))
                        .Returns(true);

            // Act
            campaign.UpdateStatus(CampaignStatus.Active, mockValidator.Object);

            // Assert
            Assert.Equal(CampaignStatus.Active, campaign.Status);
        }

        [Fact]
        public void UpdateStatus_WithInvalidTransition_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var campaign = CreateValidCampaign();
            var mockValidator = new Mock<ICampaignStateValidationService>();
            mockValidator.Setup(x => x.CanTransitionTo(CampaignStatus.Draft, CampaignStatus.Completed))
                        .Returns(false);
            mockValidator.Setup(x => x.ValidateTransition(CampaignStatus.Draft, CampaignStatus.Completed))
                        .Throws(new InvalidOperationException("Invalid transition"));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => 
                campaign.UpdateStatus(CampaignStatus.Completed, mockValidator.Object));
        }

        private Campaign CreateValidCampaign()
        {
            return Campaign.Create(
                Guid.NewGuid(),
                "Test Campaign",
                "Test Description",
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30),
                1000m,
                "https://example.com"
            );
        }
    }
}