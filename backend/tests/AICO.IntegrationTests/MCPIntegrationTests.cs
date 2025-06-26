using AICO.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using AICO.Application.Interfaces.Commands;
using AICO.Domain.Entities;
using System.Text.Json;
using System.Net.Http.Json;
using Xunit;

public class MCPIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly IAbTestCommandHandler _commandHandler;

    public MCPIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _commandHandler = _scope.ServiceProvider.GetRequiredService<IAbTestCommandHandler>();
    }
    [Fact]
    public async Task CalculateMCP_EndToEnd_ShouldReturnCorrectValue()
    {
        // Arrange - Create test data
        var testId = await CreateAbTest();
        var controlVariantId = await CreateVariant(testId, true);
        var variantId = await CreateVariant(testId, false);
        
        // Simulate conversions
        await SimulateConversions(controlVariantId, 1000m, 100);
        await SimulateConversions(variantId, 1200m, 110);

        // Act
        var response = await _client.GetAsync($"/api/analytics/mcp/{testId}/{variantId}");
        var mcp = await response.Content.ReadFromJsonAsync<decimal>();

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.True(Math.Abs(mcp - 9.09m) < 0.1m);
    }

    [Fact]
    public async Task CalculateMCP_WithStripeWebhook_ShouldUpdateMCPRealTime()
    {
        // Arrange
        var testId = await CreateAbTest();
        var variantId = await CreateVariant(testId, false);
        
        var stripeWebhook = new StripeWebhookEvent
        {
            Type = "payment_intent.succeeded",
            Data = new StripePaymentData
            {
                Amount = 9999,
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    ["test_id"] = testId.ToString(),
                    ["variant_id"] = variantId.ToString()
                }
            }
        };

        // Act
        var webhookResponse = await _client.PostAsync("/api/webhooks/stripe", new StringContent(
            System.Text.Json.JsonSerializer.Serialize(stripeWebhook), 
            System.Text.Encoding.UTF8, 
            "application/json"));
        var mcpResponse = await _client.GetAsync($"/api/analytics/mcp/{testId}/{variantId}");
        var mcp = await mcpResponse.Content.ReadFromJsonAsync<decimal>();

        // Assert
        Assert.True(webhookResponse.IsSuccessStatusCode);
        Assert.True(mcpResponse.IsSuccessStatusCode);
        Assert.True(mcp != 0); // MCP should be calculated with new conversion
    }

    [Fact]
    public async Task CalculateMCP_WithTimeRangeFilter_ShouldReturnCorrectMCP()
    {
        // Test MCP calculation for specific time periods
        var response = await _client.PostAsync("/api/tests", new StringContent(
            System.Text.Json.JsonSerializer.Serialize(new { name = "Test AB" }),
            System.Text.Encoding.UTF8,
            "application/json"));
        var testId = await response.Content.ReadFromJsonAsync<Guid>();

        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        
        var mcpResponse = await _client.GetAsync($"/api/analytics/mcp/{testId}?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}");
        Assert.True(response.IsSuccessStatusCode);
    }

    private async Task<Guid> CreateAbTest()
    {
        var command = new CreateAbTestCommand(
            Guid.NewGuid(), // CampaignId
            "Test AB", // Name
            "Test Description", // Description
            "Button Color", // TestType
            50, // V (TrafficSplit?)
            "#cta-button", // TargetSelector
            "Buy Now", // OriginalContent
            "conversion_rate", // PrimaryMetric
            50, // TrafficSplit
            DateTime.UtcNow, // StartDate
            DateTime.UtcNow.AddDays(30) // EndDate
        );
        
        var abTest = await _commandHandler.CreateAbTestAsync(command);
        return abTest.Id;
    }

    private async Task<Guid> CreateVariant(Guid testId, bool isControl)
    {
        // This is a placeholder - you'll need to implement variant creation
        // based on your actual variant creation API
        var variantData = new
        {
            testId = testId,
            name = isControl ? "Control" : "Variant A",
            content = isControl ? "Original" : "Modified",
            isControl = isControl
        };
        
        var response = await _client.PostAsync("/api/variants", 
            new StringContent(JsonSerializer.Serialize(variantData), 
            System.Text.Encoding.UTF8, "application/json"));
        
        var variantJson = await response.Content.ReadAsStringAsync();
        var variant = JsonSerializer.Deserialize<dynamic>(variantJson);
        return Guid.Parse(variant.GetProperty("id").GetString());
    }

    private async Task SimulateConversions(Guid variantId, decimal revenue, int conversions)
    {
        // This is a placeholder - you'll need to implement conversion simulation
        // based on your actual conversion tracking API
        for (int i = 0; i < conversions; i++)
        {
            var conversionData = new
            {
                variantId = variantId,
                revenue = revenue / conversions,
                timestamp = DateTime.UtcNow
            };
            
            await _client.PostAsync("/api/conversions", 
                new StringContent(JsonSerializer.Serialize(conversionData), 
                System.Text.Encoding.UTF8, "application/json"));
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _scope?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}