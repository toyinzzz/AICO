using AICO.Application.Interfaces.Commands;
using AICO.Application.Interfaces.Queries;
using AICO.Domain.Entities;
using AICO.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AICO.IntegrationTests
{
    public class AbTestIntegrationTests : IClassFixture<TestWebApplicationFactory>, IDisposable
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly IServiceScope _scope;
        private readonly IAbTestCommandHandler _commandHandler;
        private readonly IAbTestQueryHandler _queryHandler;

        public AbTestIntegrationTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _commandHandler = _scope.ServiceProvider.GetRequiredService<IAbTestCommandHandler>();
            _queryHandler = _scope.ServiceProvider.GetRequiredService<IAbTestQueryHandler>();
        }

        [Fact]
        public async Task CreateAndRetrieveAbTest_ShouldWorkEndToEnd()
        {
            // Arrange  
            var campaignId = Guid.NewGuid();
            var command = new CreateAbTestCommand(
                campaignId,
                "Integration Test A/B Test",
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

            // Act  
            var createdAbTest = await _commandHandler.CreateAbTestAsync(command);
            var retrievedAbTest = await _queryHandler.GetAbTestByIdAsync(createdAbTest.Id);

            // Assert  
            Assert.NotNull(createdAbTest);
            Assert.NotNull(retrievedAbTest);
            Assert.Equal(createdAbTest.Id, retrievedAbTest.Id);
            Assert.Equal(command.Name, retrievedAbTest.Name);
            Assert.Equal(AbTestStatus.Draft, retrievedAbTest.Status);
            Assert.Equal(command.Description, retrievedAbTest.Description);
            Assert.Equal(command.TrafficSplit, retrievedAbTest.TrafficPercentage);
        }

        [Fact]
        public async Task AbTestWorkflow_CreateStartStopComplete_ShouldWorkCorrectly()
        {
            // Arrange  
            var campaignId = Guid.NewGuid();
            var command = new CreateAbTestCommand(
                campaignId,
                "Workflow Test",
                "Test Description",
                "Text Content",
                60,
                "#cta-button",
                "Buy Now",
                "click_rate",
                60,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30)
            );

            // Act  
            var abTest = await _commandHandler.CreateAbTestAsync(command);
            Assert.Equal(AbTestStatus.Draft, abTest.Status);

            // Start Test  
            await _commandHandler.StartAbTestAsync(abTest.Id);
            var runningAbTest = await _queryHandler.GetAbTestByIdAsync(abTest.Id);
            Assert.Equal(AbTestStatus.Running, runningAbTest.Status);

            // Stop Test  
            await _commandHandler.StopAbTestAsync(abTest.Id);
            var stoppedAbTest = await _queryHandler.GetAbTestByIdAsync(abTest.Id);
            Assert.Equal(AbTestStatus.Stopped, stoppedAbTest.Status);

            // Complete Test  
            await _commandHandler.CompleteAbTestAsync(abTest.Id);
            var completedAbTest = await _queryHandler.GetAbTestByIdAsync(abTest.Id);
            Assert.Equal(AbTestStatus.Completed, completedAbTest.Status);
        }

        [Fact]
        public async Task CreateAbTest_WithInvalidTrafficSplit_ShouldFail()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var command = new CreateAbTestCommand(
                campaignId,
                "Invalid Traffic Test",
                "Test Description",
                "Button Color",
                150, // Invalid traffic split
                "#cta-button",
                "Buy Now",
                "conversion_rate",
                150,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30)
            );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _commandHandler.CreateAbTestAsync(command);
            });
        }

        [Fact]
        public async Task StartAbTest_WithoutRequiredFields_ShouldFail()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var command = new CreateAbTestCommand(
                campaignId,
                "Missing Fields Test",
                null,
                "Button Color",
                50,
                "#cta-button",
                "Buy Now",
                null, // Missing primary metric
                50,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30)
            );

            AbTest abTest = await _commandHandler.CreateAbTestAsync(command);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _commandHandler.StartAbTestAsync(abTest.Id);
            });
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}