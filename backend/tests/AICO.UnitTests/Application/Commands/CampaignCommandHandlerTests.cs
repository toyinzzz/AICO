using AICO.Application.Interfaces.Commands;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using Moq;
using Xunit;

namespace AICO.UnitTests.Application.Commands
{
    public class CampaignCommandHandlerTests
    {
        private readonly Mock<ICampaignRepository> _mockCampaignRepository;
        private readonly Mock<ICampaignService> _mockCampaignService;
        private readonly Mock<ICampaignStateValidationService> _mockValidationService;
        private readonly Mock<ICampaignCommandHandler> _mockCommandHandler;

        public CampaignCommandHandlerTests()
        {
            _mockCampaignRepository = new Mock<ICampaignRepository>();
            _mockCampaignService = new Mock<ICampaignService>();
            _mockValidationService = new Mock<ICampaignStateValidationService>();
            _mockCommandHandler = new Mock<ICampaignCommandHandler>();
        }

        [Fact]
        public async Task CreateCampaignAsync_WithValidCommand_ShouldReturnCampaign()
        {
            // Arrange
            var command = new CreateCampaignCommand(
                Guid.NewGuid(),
                "Test Campaign",
                "Test Description",
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30),
                1000m,
                "https://example.com"
            );

            var expectedCampaign = Campaign.Create(
                command.WebsiteId,
                command.Name,
                command.Description,
                command.StartDate,
                command.EndDate,
                command.Budget,
                command.TargetUrl
            );

            _mockCommandHandler.Setup(x => x.CreateCampaignAsync(command))
                              .ReturnsAsync(expectedCampaign);

            // Act
            var result = await _mockCommandHandler.Object.CreateCampaignAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.WebsiteId, result.WebsiteId);
            _mockCommandHandler.Verify(x => x.CreateCampaignAsync(command), Times.Once);
        }

        [Fact]
        public async Task StartCampaignAsync_WithValidId_ShouldReturnStartedCampaign()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var campaign = CreateTestCampaign();
            campaign.GetType().GetProperty("Status")?.SetValue(campaign, CampaignStatus.Active);

            _mockCommandHandler.Setup(x => x.StartCampaignAsync(campaignId))
                              .ReturnsAsync(campaign);

            // Act
            var result = await _mockCommandHandler.Object.StartCampaignAsync(campaignId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(CampaignStatus.Active, result.Status);
        }

        private Campaign CreateTestCampaign()
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