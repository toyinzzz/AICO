using AICO.API.Controllers;
using AICO.Application.DTOs;
using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;

namespace AICO.IntegrationTests;

/// <summary>
/// Integration tests for MCP Analytics functionality
/// Tests end-to-end MCP calculation workflows
/// </summary>
public class MCPAnalyticsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MCPAnalyticsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    #region End-to-End MCP Calculation Tests

    [Fact]
    public async Task MCPCalculation_EndToEnd_ShouldReturnCorrectValue()
    {
        // Arrange
        var request = new MCPCalculationRequest
        {
            ControlRevenue = 10000m,
            ControlConversions = 1000,
            VariantRevenue = 11000m,
            VariantConversions = 1100
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/mcp/calculate", request);
        var result = await response.Content.ReadFromJsonAsync<MCPCalculationResponse>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.True(Math.Abs(result.MCP - 10.0m) < 0.1m, $"Expected ~10% MCP, got {result.MCP}%");
        Assert.True(result.IsStatisticallySignificant);
    }

    [Fact]
    public async Task MCPCalculation_WithInsufficientData_ShouldReturnNotSignificant()
    {
        // Arrange
        var request = new MCPCalculationRequest
        {
            ControlRevenue = 100m,
            ControlConversions = 10,
            VariantRevenue = 110m,
            VariantConversions = 11
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/mcp/calculate", request);
        var result = await response.Content.ReadFromJsonAsync<MCPCalculationResponse>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.False(result.IsStatisticallySignificant);
    }

    #endregion

    #region Multiple Variants Integration Tests

    [Fact]
    public async Task MCPCalculation_MultipleVariants_ShouldReturnAllResults()
    {
        // Arrange
        var variants = new List<VariantData>
        {
            new() { VariantId = Guid.Empty, Revenue = 10000m, Conversions = 1000, IsControl = true },
            new() { VariantId = Guid.NewGuid(), Revenue = 11000m, Conversions = 1100, IsControl = false },
            new() { VariantId = Guid.NewGuid(), Revenue = 12000m, Conversions = 1200, IsControl = false }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/mcp/calculate-multiple", variants);
        var results = await response.Content.ReadFromJsonAsync<List<MCPResult>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(results);
        Assert.Equal(2, results.Count); // Should have 2 non-control variants
        Assert.All(results, r => Assert.True(r.MCP > 0));
        Assert.All(results, r => Assert.True(r.IsStatisticallySignificant));
    }

    #endregion

    #region Revenue Report Integration Tests

    [Fact]
    public async Task GenerateRevenueReport_WithMCPAnalysis_ShouldReturnComprehensiveData()
    {
        // Arrange
        var abTestId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-dd");
        var endDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

        // First, seed some test data
        await SeedTestRevenueData(abTestId);

        // Act
        var response = await _client.GetAsync($"/api/mcp/revenue-report/{abTestId}?startDate={startDate}&endDate={endDate}");
        var report = await response.Content.ReadFromJsonAsync<RevenueReport>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(report);
        Assert.Equal(abTestId, report.AbTestId);
        Assert.True(report.TotalRevenue > 0);
        Assert.True(report.TotalConversions > 0);
        Assert.NotNull(report.MCPByVariant);
        Assert.True(report.MCPByVariant.Count > 0);
    }

    #endregion

    #region MCP Trends Integration Tests

    [Theory]
    [InlineData("daily")]
    [InlineData("weekly")]
    [InlineData("monthly")]
    public async Task GetMCPTrends_WithDifferentIntervals_ShouldReturnTrendData(string interval)
    {
        // Arrange
        var abTestId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-dd");
        var endDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

        // Seed test data
        await SeedTestRevenueData(abTestId);

        // Act
        var response = await _client.GetAsync($"/api/mcp/trends/{abTestId}?startDate={startDate}&endDate={endDate}&interval={interval}");
        var trends = await response.Content.ReadFromJsonAsync<List<MCPTrendPoint>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(trends);
        Assert.True(trends.Count > 0);
        Assert.All(trends, t => Assert.Equal(interval, t.Period));
    }

    #endregion

    #region Currency Normalization Integration Tests

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    public async Task MCPCalculation_WithDifferentCurrencies_ShouldNormalizeCorrectly(string currency)
    {
        // Arrange
        var request = new MCPCalculationWithCurrencyRequest
        {
            ControlRevenue = 1000m,
            ControlConversions = 100,
            VariantRevenue = 1100m,
            VariantConversions = 110,
            Currency = currency
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/mcp/calculate-with-currency", request);
        var result = await response.Content.ReadFromJsonAsync<MCPCalculationResponse>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.True(result.MCP > 0);
        Assert.Equal(currency, result.Currency);
    }

    #endregion

    #region Confidence Interval Integration Tests

    [Theory]
    [InlineData(0.90)]
    [InlineData(0.95)]
    [InlineData(0.99)]
    public async Task MCPCalculation_WithConfidenceIntervals_ShouldReturnValidIntervals(double confidenceLevel)
    {
        // Arrange
        var request = new MCPConfidenceRequest
        {
            ControlRevenue = 10000m,
            ControlConversions = 1000,
            VariantRevenue = 11000m,
            VariantConversions = 1100,
            ConfidenceLevel = confidenceLevel
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/mcp/calculate-with-confidence", request);
        var result = await response.Content.ReadFromJsonAsync<MCPConfidenceInterval>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.Equal(confidenceLevel, result.ConfidenceLevel);
        Assert.True(result.LowerBound <= result.MCP);
        Assert.True(result.MCP <= result.UpperBound);
    }

    #endregion

    #region Error Handling Integration Tests

    [Fact]
    public async Task MCPCalculation_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new MCPCalculationRequest
        {
            ControlRevenue = -1000m, // Invalid negative revenue
            ControlConversions = 100,
            VariantRevenue = 1100m,
            VariantConversions = 110
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/mcp/calculate", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task MCPCalculation_WithMissingData_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new { }; // Empty request

        // Act
        var response = await _client.PostAsJsonAsync("/api/mcp/calculate", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Performance Integration Tests

    [Fact]
    public async Task MCPCalculation_WithLargeDataset_ShouldCompleteWithinTimeLimit()
    {
        // Arrange
        var request = new MCPCalculationRequest
        {
            ControlRevenue = 1000000m,
            ControlConversions = 100000,
            VariantRevenue = 1100000m,
            VariantConversions = 110000
        };

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var response = await _client.PostAsJsonAsync("/api/mcp/calculate", request);
        var result = await response.Content.ReadFromJsonAsync<MCPCalculationResponse>();

        // Assert
        stopwatch.Stop();
        response.EnsureSuccessStatusCode();
        Assert.True(stopwatch.ElapsedMilliseconds < 5000, "MCP calculation should complete within 5 seconds");
        Assert.NotNull(result);
    }

    #endregion

    #region Helper Methods

    private async Task SeedTestRevenueData(Guid abTestId)
    {
        using var scope = _factory.Services.CreateScope();
        var revenueRepository = scope.ServiceProvider.GetRequiredService<IRevenueRepository>();

        var revenues = new List<Revenue>
        {
            new()
            {
                Id = Guid.NewGuid(),
                AbTestId = abTestId,
                VariantId = Guid.Empty, // Control
                Amount = 100m,
                Currency = "USD",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new()
            {
                Id = Guid.NewGuid(),
                AbTestId = abTestId,
                VariantId = Guid.NewGuid(), // Variant
                Amount = 110m,
                CurrencyCode = "USD",
                CreatedAt = DateTime.UtcNow.AddDays(-9)
            }
        };

        foreach (var revenue in revenues)
        {
            await revenueRepository.AddAsync(revenue);
        }
    }

    #endregion
}

#region Request/Response Models for Integration Tests

public class MCPCalculationWithCurrencyRequest : MCPCalculationRequest
{
    public string Currency { get; set; } = "USD";
}

public class MCPConfidenceRequest : MCPCalculationRequest
{
    public double ConfidenceLevel { get; set; } = 0.95;
}

#endregion