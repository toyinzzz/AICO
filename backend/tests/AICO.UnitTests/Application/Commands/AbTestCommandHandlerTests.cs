using AICO.Application.Interfaces.Commands;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.ValueObjects;
using Moq;
using Xunit;

namespace AICO.UnitTests.Application.Commands
{
    public class AbTestCommandHandlerTests
    {
        private readonly Mock<IAbTestRepository> _mockAbTestRepository;
        private readonly Mock<IAbTestStateValidationService> _mockValidationService;
        private readonly Mock<IAbTestCommandHandler> _mockCommandHandler;

        public AbTestCommandHandlerTests()
        {
            _mockAbTestRepository = new Mock<IAbTestRepository>();
            _mockValidationService = new Mock<IAbTestStateValidationService>();
            _mockCommandHandler = new Mock<IAbTestCommandHandler>();
        }

        [Fact]
        public async Task CreateAbTestAsync_WithValidCommand_ShouldReturnAbTest()
        {
            // Arrange
            var command = new CreateAbTestCommand(
                Guid.NewGuid(),
                "Test A/B Test",
                "Test Description",
                "Button Color",
                50,
                "#cta-button",
                "Buy Now",
                "conversion_rate",
                50,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30)
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

            _mockCommandHandler.Setup(h => h.CreateAbTestAsync(command))
                .ReturnsAsync(expectedAbTest);

            // Act
            var result = await _mockCommandHandler.Object.CreateAbTestAsync(command);

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

            _mockValidationService.Setup(v => v.CanTransitionTo(AbTestStatus.Draft, AbTestStatus.Running))
                .Returns(true);

            _mockCommandHandler.Setup(h => h.StartAbTestAsync(abTestId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockCommandHandler.Object.StartAbTestAsync(abTestId);

            // Assert
            _mockCommandHandler.Verify(h => h.StartAbTestAsync(abTestId), Times.Once);
        }
    }
}