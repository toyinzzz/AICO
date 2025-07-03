using AICO.Application.Interfaces.Services;
using AICO.Application.Services;
using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AICO.UnitTests.Services;

/// <summary>
/// Comprehensive unit tests for MCP Analytics Service
/// Covers all MVP MCP functionality with edge cases
/// </summary>
public class MCPAnalyticsServiceTests
{
    private readonly Mock<IRevenueRepository> _mockRevenueRepository;
    private readonly Mock<IAbTestRepository> _mockAbTestRepository;
    private readonly Mock<ILogger<MCPAnalyticsService>> _mockLogger;
    private readonly IMCPAnalyticsService _mcpAnalyticsService;

    public MCPAnalyticsServiceTests()
    {
        _mockRevenueRepository = new Mock<IRevenueRepository>();
        _mockAbTestRepository = new Mock<IAbTestRepository>();
        _mockLogger = new Mock<ILogger<MCPAnalyticsService>>();
        
        // Note: MCPAnalyticsService implementation will be created after tests
        // _mcpAnalyticsService = new MCPAnalyticsService(_mockRevenueRepository.Object, _mockAbTestRepository.Object, _mockLogger.Object);
    }

    #region Basic MCP Calculation Tests

    [Theory]
    [InlineData(1000, 100, 1100, 110, 10.0)] // 10% improvement
    [InlineData(2000, 200, 2200, 200, 10.0)] // Same conversions, higher revenue
    [InlineData(1000, 100, 900, 90, 0.0)] // Same RPC, no improvement
    [InlineData(1000, 100, 1050, 100, 5.0)] // 5% improvement
    public void CalculateMCP_WithValidInputs_ShouldReturnCorrectPercentage(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, decimal expectedMCP)
    {
        // Act
        var result = _mcpAnalyticsService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);

        // Assert
        Assert.True(Math.Abs(result - expectedMCP) < 0.1m, $"Expected {expectedMCP}%, got {result}%");
    }

    [Theory]
    [InlineData(-1000, 100, 1100, 110)] // Negative control revenue
    [InlineData(1000, -100, 1100, 110)] // Negative control conversions
    [InlineData(1000, 100, -1100, 110)] // Negative variant revenue
    [InlineData(1000, 100, 1100, -110)] // Negative variant conversions
    public void CalculateMCP_WithNegativeInputs_ShouldThrowArgumentException(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _mcpAnalyticsService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions));
    }

    [Theory]
    [InlineData(0, 0, 1000, 100)] // Zero control values
    [InlineData(1000, 100, 0, 0)] // Zero variant values
    [InlineData(0, 0, 0, 0)] // All zero values
    public void CalculateMCP_WithZeroValues_ShouldHandleGracefully(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
    {
        // Act
        var result = _mcpAnalyticsService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);

        // Assert
        Assert.True(result >= -100 && result <= 10000, "MCP should be within reasonable bounds");
    }

    #endregion

    #region Currency Normalization Tests

    [Theory]
    [InlineData("USD", 1000, 100, 1100, 110, 10.0)]
    [InlineData("EUR", 850, 100, 935, 110, 10.0)] // Assuming EUR conversion
    [InlineData("GBP", 750, 100, 825, 110, 10.0)] // Assuming GBP conversion
    public void CalculateMCPWithCurrency_WithDifferentCurrencies_ShouldNormalizeCorrectly(string currency, decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, decimal expectedMCP)
    {
        // Act
        var result = _mcpAnalyticsService.CalculateMCPWithCurrency(controlRevenue, controlConversions, variantRevenue, variantConversions, currency);

        // Assert
        Assert.True(Math.Abs(result - expectedMCP) < 0.5m, $"Expected ~{expectedMCP}% for {currency}, got {result}%");
    }

    [Fact]
    public void CalculateMCPWithCurrency_WithUnsupportedCurrency_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _mcpAnalyticsService.CalculateMCPWithCurrency(1000, 100, 1100, 110, "INVALID"));
    }

    #endregion

    #region Multiple Variants Tests

    [Fact]
    public void CalculateMCPForMultipleVariants_WithValidVariants_ShouldReturnCorrectResults()
    {
        // Arrange
        var variants = new List<VariantData>
        {
            new() { VariantId = Guid.Empty, Revenue = 1000, Conversions = 100, IsControl = true },
            new() { VariantId = Guid.NewGuid(), Revenue = 1100, Conversions = 110, IsControl = false },
            new() { VariantId = Guid.NewGuid(), Revenue = 1200, Conversions = 120, IsControl = false }
        };

        // Act
        var results = _mcpAnalyticsService.CalculateMCPForMultipleVariants(variants);

        // Assert
        Assert.Equal(2, results.Count); // Should have 2 non-control variants
        Assert.All(results, r => Assert.True(r.MCP > 0)); // All variants should show improvement
        Assert.All(results, r => Assert.NotEqual(Guid.Empty, r.VariantId)); // No control variant in results
    }

    [Fact]
    public void CalculateMCPForMultipleVariants_WithNoControlVariant_ShouldThrowArgumentException()
    {
        // Arrange
        var variants = new List<VariantData>
        {
            new() { VariantId = Guid.NewGuid(), Revenue = 1100, Conversions = 110, IsControl = false },
            new() { VariantId = Guid.NewGuid(), Revenue = 1200, Conversions = 120, IsControl = false }
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _mcpAnalyticsService.CalculateMCPForMultipleVariants(variants));
    }

    #endregion

    #region Statistical Significance Tests

    [Fact]
    public void CalculateMCPWithSignificance_WithInsufficientSampleSize_ShouldReturnNull()
    {
        // Arrange - Sample size below minimum threshold (30)
        var controlRevenue = 100m;
        var controlConversions = 10;
        var variantRevenue = 110m;
        var variantConversions = 11;

        // Act
        var result = _mcpAnalyticsService.CalculateMCPWithSignificance(controlRevenue, controlConversions, variantRevenue, variantConversions);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CalculateMCPWithSignificance_WithSufficientSampleSize_ShouldReturnValue()
    {
        // Arrange - Sample size above minimum threshold
        var controlRevenue = 3000m;
        var controlConversions = 300;
        var variantRevenue = 3300m;
        var variantConversions = 330;

        // Act
        var result = _mcpAnalyticsService.CalculateMCPWithSignificance(controlRevenue, controlConversions, variantRevenue, variantConversions);

        // Assert
        Assert.NotNull(result);
        Assert.True(result > 0);
    }

    #endregion

    #region MCP Analysis Report Tests

    [Fact]
    public async Task GenerateMCPAnalysisAsync_WithValidData_ShouldReturnComprehensiveReport()
    {
        // Arrange
        var abTestId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-30);
        var endDate = DateTime.UtcNow;

        var mockRevenues = new List<Revenue>
        {
            new() { AbTestId = abTestId, VariantId = Guid.Empty, Amount = 100, Currency = "USD", CreatedAt = startDate.AddDays(1) },
            new() { AbTestId = abTestId, VariantId = Guid.NewGuid(), Amount = 110, Currency = "USD", CreatedAt = startDate.AddDays(2) }
        };

        _mockRevenueRepository.Setup(r => r.GetRevenueByAbTestAsync(abTestId, startDate, endDate))
            .ReturnsAsync(mockRevenues);

        // Act
        var report = await _mcpAnalyticsService.GenerateMCPAnalysisAsync(abTestId, startDate, endDate);

        // Assert
        Assert.NotNull(report);
        Assert.Equal(abTestId, report.AbTestId);
        Assert.Equal(startDate, report.StartDate);
        Assert.Equal(endDate, report.EndDate);
        Assert.True(report.TotalRevenue > 0);
        Assert.True(report.VariantMCPResults.Count > 0);
    }

    #endregion

    #region MCP Trends Tests

    [Theory]
    [InlineData("daily")]
    [InlineData("weekly")]
    [InlineData("monthly")]
    public async Task GetMCPTrendsAsync_WithDifferentIntervals_ShouldReturnCorrectTrends(string interval)
    {
        // Arrange
        var abTestId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-30);
        var endDate = DateTime.UtcNow;

        // Act
        var trends = await _mcpAnalyticsService.GetMCPTrendsAsync(abTestId, startDate, endDate, interval);

        // Assert
        Assert.NotNull(trends);
        Assert.All(trends, t => Assert.Equal(interval, t.Period));
        Assert.All(trends, t => Assert.True(t.Timestamp >= startDate && t.Timestamp <= endDate));
    }

    #endregion

    #region Validation Tests

    [Theory]
    [InlineData(1000, 100, 1100, 110, true)] // Valid inputs
    [InlineData(-1000, 100, 1100, 110, false)] // Invalid: negative revenue
    [InlineData(1000, -100, 1100, 110, false)] // Invalid: negative conversions
    [InlineData(1000, 5, 1100, 6, false)] // Invalid: insufficient sample size
    public void ValidateMCPInputs_WithVariousInputs_ShouldReturnCorrectValidation(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, bool expectedValid)
    {
        // Act
        var result = _mcpAnalyticsService.ValidateMCPInputs(controlRevenue, controlConversions, variantRevenue, variantConversions);

        // Assert
        Assert.Equal(expectedValid, result.IsValid);
        if (!expectedValid)
        {
            Assert.True(result.ErrorMessages.Count > 0);
        }
    }

    #endregion

    #region Confidence Interval Tests

    [Theory]
    [InlineData(0.90)] // 90% confidence
    [InlineData(0.95)] // 95% confidence
    [InlineData(0.99)] // 99% confidence
    public void CalculateMCPWithConfidence_WithDifferentConfidenceLevels_ShouldReturnValidIntervals(double confidenceLevel)
    {
        // Arrange
        var controlRevenue = 10000m;
        var controlConversions = 1000;
        var variantRevenue = 11000m;
        var variantConversions = 1100;

        // Act
        var result = _mcpAnalyticsService.CalculateMCPWithConfidence(controlRevenue, controlConversions, variantRevenue, variantConversions, confidenceLevel);

        // Assert
        Assert.Equal(confidenceLevel, result.ConfidenceLevel);
        Assert.True(result.LowerBound <= result.MCP);
        Assert.True(result.MCP <= result.UpperBound);
        Assert.True(result.MarginOfError > 0);
    }

    #endregion

    #region Performance Tests

    [Fact]
    public void CalculateMCP_WithLargeNumbers_ShouldCompleteQuickly()
    {
        // Arrange
        var controlRevenue = decimal.MaxValue / 2;
        var controlConversions = int.MaxValue / 2;
        var variantRevenue = decimal.MaxValue / 2 * 1.1m;
        var variantConversions = int.MaxValue / 2;

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var result = _mcpAnalyticsService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);

        // Assert
        stopwatch.Stop();
        Assert.True(stopwatch.ElapsedMilliseconds < 100, "MCP calculation should complete within 100ms");
        Assert.True(result > 0);
    }

    #endregion
}