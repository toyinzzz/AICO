using AICO.Application.DTOs;
using AICO.Application.Interfaces.Services;
using AICO.Application.Services;
using AICO.Domain.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AICO.UnitTests.Services;

/// <summary>
/// Comprehensive unit tests for MCP Validation Service
/// Ensures proper validation of all MCP calculation inputs and business rules
/// </summary>
public class MCPValidationServiceTests
{
    private readonly Mock<ILogger<MCPValidationService>> _mockLogger;
    private readonly IMCPValidationService _validationService;

    public MCPValidationServiceTests()
    {
        _mockLogger = new Mock<ILogger<MCPValidationService>>();
        
        // Note: MCPValidationService implementation will be created after tests
        // _validationService = new MCPValidationService(_mockLogger.Object);
    }

    #region Basic Input Validation Tests

    [Theory]
    [InlineData(1000, 100, 1100, 110, true)] // Valid inputs
    [InlineData(0, 100, 1100, 110, false)] // Zero control revenue
    [InlineData(1000, 0, 1100, 110, false)] // Zero control conversions
    [InlineData(1000, 100, 0, 110, false)] // Zero variant revenue
    [InlineData(1000, 100, 1100, 0, false)] // Zero variant conversions
    [InlineData(-1000, 100, 1100, 110, false)] // Negative control revenue
    [InlineData(1000, -100, 1100, 110, false)] // Negative control conversions
    [InlineData(1000, 100, -1100, 110, false)] // Negative variant revenue
    [InlineData(1000, 100, 1100, -110, false)] // Negative variant conversions
    public void ValidateBasicInputs_WithVariousInputs_ShouldReturnCorrectValidation(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, bool expectedValid)
    {
        // Act
        var result = _validationService.ValidateBasicInputs(controlRevenue, controlConversions, variantRevenue, variantConversions);

        // Assert
        Assert.Equal(expectedValid, result.IsValid);
        if (!expectedValid)
        {
            Assert.True(result.ErrorMessages.Count > 0, "Should have error messages for invalid inputs");
        }
    }

    [Fact]
    public void ValidateBasicInputs_WithExtremelyLargeValues_ShouldValidateCorrectly()
    {
        // Arrange
        var controlRevenue = decimal.MaxValue / 2;
        var controlConversions = int.MaxValue / 2;
        var variantRevenue = decimal.MaxValue / 2;
        var variantConversions = int.MaxValue / 2;

        // Act
        var result = _validationService.ValidateBasicInputs(controlRevenue, controlConversions, variantRevenue, variantConversions);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ErrorMessages);
    }

    #endregion

    #region Statistical Significance Validation Tests

    [Theory]
    [InlineData(100, 110, 30, true)] // Sufficient sample size
    [InlineData(10, 11, 30, false)] // Insufficient sample size
    [InlineData(50, 60, 100, false)] // Insufficient for higher threshold
    [InlineData(1000, 1100, 30, true)] // Large sample size
    public void ValidateStatisticalSignificance_WithVariousSampleSizes_ShouldReturnCorrectValidation(int controlConversions, int variantConversions, int minimumSampleSize, bool expectedValid)
    {
        // Act
        var result = _validationService.ValidateStatisticalSignificance(controlConversions, variantConversions, minimumSampleSize);

        // Assert
        Assert.Equal(expectedValid, result.HasSufficientSampleSize);
        Assert.Equal(controlConversions + variantConversions, result.ActualSampleSize);
        Assert.Equal(minimumSampleSize, result.RequiredSampleSize);
    }

    [Fact]
    public void ValidateStatisticalSignificance_ShouldCalculatePowerAnalysis()
    {
        // Arrange
        var controlConversions = 1000;
        var variantConversions = 1100;
        var minimumSampleSize = 30;

        // Act
        var result = _validationService.ValidateStatisticalSignificance(controlConversions, variantConversions, minimumSampleSize);

        // Assert
        Assert.True(result.PowerAnalysis >= 0 && result.PowerAnalysis <= 1, "Power analysis should be between 0 and 1");
        Assert.NotEmpty(result.RecommendedAction);
    }

    #endregion

    #region Currency Validation Tests

    [Theory]
    [InlineData("USD", true)]
    [InlineData("EUR", true)]
    [InlineData("GBP", true)]
    [InlineData("JPY", true)]
    [InlineData("INVALID", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("usd", false)] // Case sensitive
    public void ValidateCurrency_WithVariousCurrencies_ShouldReturnCorrectValidation(string currency, bool expectedValid)
    {
        // Act
        var result = _validationService.ValidateCurrency(currency);

        // Assert
        Assert.Equal(expectedValid, result.IsValid);
        Assert.Equal(expectedValid, result.IsSupported);
        if (!expectedValid)
        {
            Assert.True(result.ErrorMessages.Count > 0);
        }
        Assert.True(result.SupportedCurrencies.Count > 0);
    }

    #endregion

    #region Multiple Variants Validation Tests

    [Fact]
    public void ValidateMultipleVariants_WithValidVariants_ShouldReturnValid()
    {
        // Arrange
        var variants = new List<VariantData>
        {
            new() { VariantId = Guid.Empty, Revenue = 1000, Conversions = 100, IsControl = true },
            new() { VariantId = Guid.NewGuid(), Revenue = 1100, Conversions = 110, IsControl = false },
            new() { VariantId = Guid.NewGuid(), Revenue = 1200, Conversions = 120, IsControl = false }
        };

        // Act
        var result = _validationService.ValidateMultipleVariants(variants);

        // Assert
        Assert.True(result.IsValid);
        Assert.True(result.HasControlVariant);
        Assert.Equal(3, result.VariantCount);
        Assert.Equal(3, result.ValidVariantCount);
        Assert.Empty(result.VariantErrors);
    }

    [Fact]
    public void ValidateMultipleVariants_WithoutControlVariant_ShouldReturnInvalid()
    {
        // Arrange
        var variants = new List<VariantData>
        {
            new() { VariantId = Guid.NewGuid(), Revenue = 1100, Conversions = 110, IsControl = false },
            new() { VariantId = Guid.NewGuid(), Revenue = 1200, Conversions = 120, IsControl = false }
        };

        // Act
        var result = _validationService.ValidateMultipleVariants(variants);

        // Assert
        Assert.False(result.IsValid);
        Assert.False(result.HasControlVariant);
        Assert.True(result.GeneralErrors.Count > 0);
        Assert.Contains("control", result.GeneralErrors[0].ToLower());
    }

    [Fact]
    public void ValidateMultipleVariants_WithInvalidVariantData_ShouldReturnVariantErrors()
    {
        // Arrange
        var variants = new List<VariantData>
        {
            new() { VariantId = Guid.Empty, Revenue = 1000, Conversions = 100, IsControl = true },
            new() { VariantId = Guid.NewGuid(), Revenue = -1100, Conversions = 110, IsControl = false }, // Invalid revenue
            new() { VariantId = Guid.NewGuid(), Revenue = 1200, Conversions = -120, IsControl = false } // Invalid conversions
        };

        // Act
        var result = _validationService.ValidateMultipleVariants(variants);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.VariantErrors.Count);
        Assert.Equal(1, result.ValidVariantCount);
    }

    #endregion

    #region Date Range Validation Tests

    [Fact]
    public void ValidateDateRange_WithValidRange_ShouldReturnValid()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(-30);
        var endDate = DateTime.UtcNow;

        // Act
        var result = _validationService.ValidateDateRange(startDate, endDate);

        // Assert
        Assert.True(result.IsValid);
        Assert.True(result.IsReasonableDuration);
        Assert.False(result.IsInFuture);
        Assert.True((result.Duration - TimeSpan.FromDays(30)).Duration() <= TimeSpan.FromHours(1));
    }

    [Fact]
    public void ValidateDateRange_WithEndBeforeStart_ShouldReturnInvalid()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddDays(-30);

        // Act
        var result = _validationService.ValidateDateRange(startDate, endDate);

        // Assert
        Assert.False(result.IsValid);
        Assert.True(result.ValidationMessages.Count > 0);
    }

    [Fact]
    public void ValidateDateRange_WithFutureDates_ShouldReturnWarning()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(30);

        // Act
        var result = _validationService.ValidateDateRange(startDate, endDate);

        // Assert
        Assert.True(result.IsInFuture);
        Assert.True(result.ValidationMessages.Count > 0);
    }

    #endregion

    #region Confidence Level Validation Tests

    [Theory]
    [InlineData(0.90, true, true)] // Standard 90%
    [InlineData(0.95, true, true)] // Standard 95%
    [InlineData(0.99, true, true)] // Standard 99%
    [InlineData(0.85, true, false)] // Valid but non-standard
    [InlineData(0.999, true, false)] // Valid but non-standard
    [InlineData(1.5, false, false)] // Invalid > 1
    [InlineData(-0.5, false, false)] // Invalid < 0
    [InlineData(0.0, false, false)] // Invalid zero
    public void ValidateConfidenceLevel_WithVariousLevels_ShouldReturnCorrectValidation(double confidenceLevel, bool expectedValid, bool expectedStandard)
    {
        // Act
        var result = _validationService.ValidateConfidenceLevel(confidenceLevel);

        // Assert
        Assert.Equal(expectedValid, result.IsValid);
        Assert.Equal(expectedStandard, result.IsStandardLevel);
        Assert.Equal(confidenceLevel, result.ConfidenceLevel);
        Assert.True(result.RecommendedLevels.Count > 0);
    }

    #endregion

    #region A/B Test Validation Tests

    [Fact]
    public async Task ValidateAbTestConfigurationAsync_WithValidTest_ShouldReturnValid()
    {
        // Arrange
        var abTestId = Guid.NewGuid();
        var variants = new List<VariantData>
        {
            new() { VariantId = Guid.Empty, Revenue = 1000, Conversions = 100, IsControl = true },
            new() { VariantId = Guid.NewGuid(), Revenue = 1100, Conversions = 110, IsControl = false }
        };

        // Act
        var result = await _validationService.ValidateAbTestConfigurationAsync(abTestId, variants);

        // Assert
        Assert.True(result.IsValid);
        Assert.True(result.TestExists);
        Assert.True(result.HasSufficientData);
        Assert.Equal(abTestId, result.AbTestId);
    }

    [Fact]
    public async Task ValidateAbTestConfigurationAsync_WithNonExistentTest_ShouldReturnInvalid()
    {
        // Arrange
        var abTestId = Guid.NewGuid();
        var variants = new List<VariantData>();

        // Act
        var result = await _validationService.ValidateAbTestConfigurationAsync(abTestId, variants);

        // Assert
        Assert.False(result.IsValid);
        Assert.False(result.TestExists);
        Assert.True(result.ValidationErrors.Count > 0);
    }

    #endregion

    #region Business Rules Validation Tests

    [Theory]
    [InlineData(10.0, true)] // Reasonable MCP
    [InlineData(50.0, true)] // High but acceptable MCP
    [InlineData(500.0, false)] // Unrealistic MCP
    [InlineData(-50.0, true)] // Negative MCP (variant performing worse)
    [InlineData(-150.0, false)] // Extremely negative MCP
    public void ValidateBusinessRules_WithVariousMCPValues_ShouldReturnCorrectValidation(decimal mcpValue, bool expectedRealistic)
    {
        // Arrange
        var businessContext = new MCPBusinessContext
        {
            Industry = "E-commerce",
            BusinessModel = "B2C",
            MaxAcceptableMCP = 100m,
            MinMeaningfulMCP = 1m
        };

        // Act
        var result = _validationService.ValidateBusinessRules(mcpValue, businessContext);

        // Assert
        Assert.Equal(expectedRealistic, result.IsRealistic);
        Assert.Equal(mcpValue, result.MCPValue);
        if (!expectedRealistic)
        {
            Assert.True(result.RuleViolations.Count > 0);
            Assert.True(result.Recommendations.Count > 0);
        }
    }

    #endregion

    #region Comprehensive Validation Tests

    [Fact]
    public async Task ValidateComprehensiveAsync_WithValidRequest_ShouldReturnValid()
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
        var result = await _validationService.ValidateComprehensiveAsync(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.True(result.CanProceedWithCalculation);
        Assert.True(result.BasicValidation.IsValid);
        Assert.True(result.StatisticalValidation.HasSufficientSampleSize);
        Assert.Empty(result.CriticalErrors);
    }

    [Fact]
    public async Task ValidateComprehensiveAsync_WithInvalidRequest_ShouldReturnInvalid()
    {
        // Arrange
        var request = new MCPCalculationRequest
        {
            ControlRevenue = -1000m, // Invalid
            ControlConversions = 10, // Insufficient sample
            VariantRevenue = 1100m,
            VariantConversions = 11 // Insufficient sample
        };

        // Act
        var result = await _validationService.ValidateComprehensiveAsync(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.False(result.CanProceedWithCalculation);
        Assert.False(result.BasicValidation.IsValid);
        Assert.False(result.StatisticalValidation.HasSufficientSampleSize);
        Assert.True(result.CriticalErrors.Count > 0);
    }

    #endregion

    #region Edge Cases and Error Handling Tests

    [Fact]
    public void ValidateBasicInputs_WithNullInputs_ShouldHandleGracefully()
    {
        // This test ensures the service handles edge cases gracefully
        // Implementation should handle any potential null reference scenarios
        
        // Act & Assert - Should not throw exceptions
        var result = _validationService.ValidateBasicInputs(0, 0, 0, 0);
        Assert.NotNull(result);
    }

    [Fact]
    public void ValidateMultipleVariants_WithEmptyList_ShouldReturnInvalid()
    {
        // Arrange
        var variants = new List<VariantData>();

        // Act
        var result = _validationService.ValidateMultipleVariants(variants);

        // Assert
        Assert.False(result.IsValid);
        Assert.True(result.GeneralErrors.Count > 0);
    }

    #endregion
}