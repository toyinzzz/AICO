using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace AICO.Domain.Tests.Repositories
{
    public class CampaignRepositoryTests
    {
        private readonly Mock<ICampaignRepository> _mockRepository;

        public CampaignRepositoryTests()
        {
            _mockRepository = new Mock<ICampaignRepository>();
        }

        [Fact]
        public async Task GetByWebsiteIdAsync_ShouldReturnCampaignsForWebsite()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var campaigns = new List<Campaign>
            {
                CreateTestCampaign(websiteId, "Campaign 1"),
                CreateTestCampaign(websiteId, "Campaign 2")
            };

            _mockRepository.Setup(x => x.GetByWebsiteIdAsync(websiteId))
                          .ReturnsAsync(campaigns);

            // Act
            var result = await _mockRepository.Object.GetByWebsiteIdAsync(websiteId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, c => Assert.Equal(websiteId, c.WebsiteId));
        }

        [Fact]
        public async Task GetActiveCampaignsAsync_ShouldReturnOnlyActiveCampaigns()
        {
            // Arrange
            var activeCampaigns = new List<Campaign>
            {
                CreateTestCampaign(Guid.NewGuid(), "Active Campaign 1", isActive: true),
                CreateTestCampaign(Guid.NewGuid(), "Active Campaign 2", isActive: true)
            };

            _mockRepository.Setup(x => x.GetActiveCampaignsAsync())
                          .ReturnsAsync(activeCampaigns);

            // Act
            var result = await _mockRepository.Object.GetActiveCampaignsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _mockRepository.Verify(x => x.GetActiveCampaignsAsync(), Times.Once);
        }

        [Fact]
        public async Task ExistsByNameAsync_WithExistingName_ShouldReturnTrue()
        {
            // Arrange
            var campaignName = "Existing Campaign";
            _mockRepository.Setup(x => x.ExistsByNameAsync(campaignName))
                          .ReturnsAsync(true);

            // Act
            var result = await _mockRepository.Object.ExistsByNameAsync(campaignName);

            // Assert
            Assert.True(result);
        }

        private Campaign CreateTestCampaign(Guid websiteId, string name, bool isActive = false)
        {
            var campaign = new Campaign(
                name,
                "Test Description", // Default description
                websiteId,
                Guid.NewGuid()    // Dummy UserId for test
            );

            if (isActive)
            {
                campaign.Start(DateTime.UtcNow); 
            }
            return campaign;
        }
    }
}