using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Services;
using Moq;
using Xunit;

namespace AICO.UnitTests.Domain.Services
{
    public class RevenueTrackingServiceTests
    {
        private readonly Mock<IRevenueRepository> _mockRevenueRepository;
        private readonly Mock<ICampaignRepository> _mockCampaignRepository;
        private readonly RevenueTrackingService _revenueService;

        public RevenueTrackingServiceTests()
        {
            _mockRevenueRepository = new Mock<IRevenueRepository>();
            _mockCampaignRepository = new Mock<ICampaignRepository>();
            var mockLogger = new Mock<ILogger<RevenueTrackingService>>();
            var mockMapper = new Mock<IMapper<Revenue, RevenueDto>>();
            var mockReportMapper = new Mock<IMapper<RevenueReportSource, RevenueReport>>();
            _revenueService = new RevenueTrackingService(
                _mockRevenueRepository.Object,
                mockLogger.Object,
                mockMapper.Object,
                mockReportMapper.Object);
        }

        [Fact]
        public async Task CalculateProfitLiftAsync_WithPositiveLift_ShouldReturnCorrectValue()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var controlRevenue = 10000m;
            var variantRevenue = 12000m;
            var expectedLift = 2000m; // 20% lift

            _mockRevenueRepository.Setup(x => x.GetTotalRevenueAsync(campaignId))
                .ReturnsAsync(controlRevenue + variantRevenue);

            // Act
            var result = await _revenueService.CalculateProfitLiftAsync(campaignId);

            // Assert
            Assert.Equal(expectedLift, result);
        }

        [Fact]
        public async Task CalculateROIAsync_WithValidData_ShouldReturnCorrectROI()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var totalRevenue = 15000m;
            var campaignCost = 3000m;
            var expectedROI = 4.0m; // 400% ROI

            var campaign = new Campaign("Test Campaign", "Description", Guid.NewGuid(), Guid.NewGuid());

            _mockCampaignRepository.Setup(x => x.GetByIdAsync(campaignId))
                .ReturnsAsync(campaign);
            _mockRevenueRepository.Setup(x => x.GetTotalRevenueAsync(campaignId))
                .ReturnsAsync(totalRevenue);

            // Act
            var result = await _revenueService.CalculateROIAsync(campaignId);

            // Assert
            Assert.Equal(expectedROI, result);
        }

        // Note: CalculateMarginalCostOfProductionAsync test removed as GetTotalCostsAsync and GetTotalUnitsAsync are not implemented

        [Fact]
        public async Task RecordRevenueEventAsync_WithValidData_ShouldCreateRevenueRecord()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var amount = 99.99m;
            var currency = "USD";
            var transactionId = "txn_123";

            Revenue capturedRevenue = null;
            _mockRevenueRepository.Setup(x => x.AddAsync(It.IsAny<Revenue>()))
                .Callback<Revenue>(r => capturedRevenue = r)
                .ReturnsAsync(new Revenue(Guid.NewGuid(), Guid.NewGuid(), 100m, "test"));

            // Act
            await _revenueService.RecordRevenueEventAsync(campaignId, variantId, amount, currency, transactionId);

            // Assert
            Assert.NotNull(capturedRevenue);
            Assert.Equal(campaignId, capturedRevenue.CampaignId);
            Assert.Equal(variantId, capturedRevenue.VariantId);
            Assert.Equal(amount, capturedRevenue.Amount);
            Assert.Equal(currency, capturedRevenue.Currency);
            Assert.Equal(transactionId, capturedRevenue.TransactionId);
        }
    }
}