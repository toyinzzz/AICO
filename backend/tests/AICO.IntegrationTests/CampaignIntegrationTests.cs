using AICO.Application.Interfaces.Commands;
using AICO.Application.Interfaces.Queries;
using AICO.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AICO.IntegrationTests
{
    public class CampaignIntegrationTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private readonly IServiceScope _scope;
        private readonly ICampaignCommandHandler _commandHandler;
        private readonly ICampaignQueryHandler _queryHandler;

        public CampaignIntegrationTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _commandHandler = _scope.ServiceProvider.GetRequiredService<ICampaignCommandHandler>();
            _queryHandler = _scope.ServiceProvider.GetRequiredService<ICampaignQueryHandler>();
        }

        [Fact]
        public async Task CreateAndRetrieveCampaign_ShouldWorkEndToEnd()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var createCommand = new CreateCampaignCommand(
                websiteId,
                "Integration Test Campaign",
                "Test Description",
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30),
                1000m,
                "https://example.com"
            );

            // Act - Create
            var createdCampaign = await _commandHandler.CreateCampaignAsync(createCommand);

            // Act - Retrieve
            var retrievedCampaign = await _queryHandler.GetCampaignByIdAsync(createdCampaign.Id);

            // Assert
            Assert.NotNull(retrievedCampaign);
            Assert.Equal(createdCampaign.Id, retrievedCampaign.Id);
            Assert.Equal(createCommand.Name, retrievedCampaign.Name);
            Assert.Equal(createCommand.WebsiteId, retrievedCampaign.WebsiteId);
        }

        [Fact]
        public async Task CampaignLifecycle_ShouldWorkEndToEnd()
        {
            // Arrange
            var createCommand = new CreateCampaignCommand(
                Guid.NewGuid(),
                "Lifecycle Test Campaign",
                "Test Description",
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30),
                1000m,
                "https://example.com"
            );

            // Act & Assert - Create
            var campaign = await _commandHandler.CreateCampaignAsync(createCommand);
            Assert.Equal(CampaignStatus.Draft, campaign.Status);

            // Act & Assert - Start
            var startedCampaign = await _commandHandler.StartCampaignAsync(campaign.Id);
            Assert.Equal(CampaignStatus.Active, startedCampaign.Status);

            // Act & Assert - Complete
            var completedCampaign = await _commandHandler.CompleteCampaignAsync(campaign.Id);
            Assert.Equal(CampaignStatus.Completed, completedCampaign.Status);
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}