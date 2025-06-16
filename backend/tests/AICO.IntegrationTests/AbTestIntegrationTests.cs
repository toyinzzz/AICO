using AICO.Application.Interfaces.Commands;
using AICO.Application.Interfaces.Queries;
using AICO.Domain.Entities;
using AICO.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AICO.IntegrationTests
{
    public class AbTestIntegrationTests : IClassFixture<TestWebApplicationFactory>
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
                TestType.Create("Button Color"),
                50,
                "conversion_rate",
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
        }

        [Fact]
        public async Task AbTestWorkflow_CreateStartStop_ShouldWorkCorrectly()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var command = new CreateAbTestCommand(
                campaignId,
                "Workflow Test",
                "Test Description",
                TestType.Create("Text Content"),
                60,
                "click_rate",
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30)
            );

            // Act & Assert
            var abTest = await _commandHandler.CreateAbTestAsync(command);
            Assert.Equal(AbTestStatus.Draft, abTest.Status);

            await _commandHandler.StartAbTestAsync(abTest.Id);
            var runningAbTest = await _queryHandler.GetAbTestByIdAsync(abTest.Id);
            Assert.Equal(AbTestStatus.Running, runningAbTest.Status);

            await _commandHandler.StopAbTestAsync(abTest.Id);
            var stoppedAbTest = await _queryHandler.GetAbTestByIdAsync(abTest.Id);
            Assert.Equal(AbTestStatus.Stopped, stoppedAbTest.Status);
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}