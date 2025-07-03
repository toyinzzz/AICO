using Xunit;
using Moq;
using AICO.Application.Commands;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Application.Interfaces.Commands;
using AICO.Domain.ValueObjects;

namespace AICO.UnitTests.Application.Commands
{
    public class AbTestCommandHandlerTests
    {
        private readonly Mock<IAbTestRepository> _mockAbTestRepository;
        private readonly AbTestCommandHandler _handler;

        public AbTestCommandHandlerTests()
        {
            _mockAbTestRepository = new Mock<IAbTestRepository>();
            _handler = new AbTestCommandHandler(_mockAbTestRepository.Object);
        }

        [Fact]
        public async Task CreateAbTestAsync_WithValidCommand_ShouldReturnAbTest()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var command = new CreateAbTestCommand(
                campaignId,           // CampaignId (Guid)
                "Test A/B Test",      // Name (string)
                "Test Description",   // Description (string)
                "Button Color",       // TestType (string)
                1,                     // V (int)
                "#cta-button",        // TargetSelector (string)
                "Buy Now",            // OriginalContent (string)
                "conversion_rate",    // PrimaryMetric (string)
                50,                    // TrafficSplit (int)
                DateTime.UtcNow.AddDays(1),   // StartDate (DateTime)
                DateTime.UtcNow.AddDays(30)   // EndDate (DateTime?)
            );

            var expectedAbTest = AbTest.Create(
                command.Name,
                command.Description,
                command.CampaignId,
                AICO.Domain.ValueObjects.TestType.Create(command.TestType),
                command.TargetSelector,
                command.OriginalContent,
                command.PrimaryMetric
            );

            _mockAbTestRepository.Setup(r => r.AddAsync(It.IsAny<AbTest>()))
                .ReturnsAsync(expectedAbTest);

            // Act
            var result = await _handler.CreateAbTestAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.CampaignId, result.CampaignId);
        }

        [Fact]
        public async Task StartAbTestAsync_WithValidId_ShouldUpdateStatus()
        {
            // Arrange
            var abTestId = Guid.NewGuid();
            var campaignId = Guid.NewGuid();
            var testType = TestType.Create("Button Color");
            var abTest = AbTest.Create(
                "Test",
                "Description",
                campaignId,
                testType,
                "#cta-button",
                "Buy Now",
                "conversion_rate"
            );

            _mockAbTestRepository.Setup(r => r.GetByIdAsync(abTestId))
                .ReturnsAsync(abTest);
            _mockAbTestRepository.Setup(r => r.UpdateAsync(It.IsAny<AbTest>()))
                .ReturnsAsync(abTest);

            // Act
            var result = await _handler.StartAbTestAsync(abTestId);

            // Assert
            Assert.NotNull(result);
            _mockAbTestRepository.Verify(r => r.UpdateAsync(It.IsAny<AbTest>()), Times.Once);
        }

        [Fact]
        public async Task StopAbTestAsync_WithValidId_ShouldUpdateStatus()
        {
            // Arrange
            var abTestId = Guid.NewGuid();
            var campaignId = Guid.NewGuid();
            var testType = TestType.Create("Button Color");
            var abTest = AbTest.Create(
                "Test",
                "Description",
                campaignId,
                testType,
                "#cta-button",
                "Buy Now",
                "conversion_rate"
            );

            _mockAbTestRepository.Setup(r => r.GetByIdAsync(abTestId))
                .ReturnsAsync(abTest);
            _mockAbTestRepository.Setup(r => r.UpdateAsync(It.IsAny<AbTest>()))
                .ReturnsAsync(abTest);

            // Act
            var result = await _handler.StopAbTestAsync(abTestId);

            // Assert
            Assert.NotNull(result);
            _mockAbTestRepository.Verify(r => r.UpdateAsync(It.IsAny<AbTest>()), Times.Once);
        }
    }
}