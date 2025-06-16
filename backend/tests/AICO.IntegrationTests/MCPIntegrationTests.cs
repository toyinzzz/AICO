using AICO.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class MCPIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MCPIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
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
        var testId = await _client.PostAsync("/api/tests", new StringContent(
            System.Text.Json.JsonSerializer.Serialize(new { name = "Test AB" }),
            System.Text.Encoding.UTF8,
            "application/json")).Result.Content.ReadAsStringAsync().Result.Deserialize<Guid>();

        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;
        
        var response = await _client.GetAsync($"/api/analytics/mcp/{testId}?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}");
        Assert.True(response.IsSuccessStatusCode);
    }
}