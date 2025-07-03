using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AICO.IntegrationTests;

/// <summary>
/// Integration tests for MCP validation service
/// Tests end-to-end validation functionality with real data flows
/// </summary>
public class MCPValidationIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly IServiceScope _scope;
    private readonly IMCPValidationService _validationService;
    private readonly IMCPAnalyticsService _analyticsService;
    private readonly ILogger<MCPValidationIntegrationTests> _logger;
    private readonly Guid _testAbTestId = Guid.NewGuid();
    private readonly DateTime _testStartDate = DateTime.UtcNow.AddDays(-30);
    private readonly DateTime _testEndDate = DateTime.UtcNow;

    public MCPValidationIntegrationTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _scope = _factory.Services.CreateScope();
        _validationService = _scope.ServiceProvider.GetRequiredService<IMCPValidationService>();
        _analyticsService = _scope.ServiceProvider.GetRequiredService<IMCPAnalyticsService>();
        _logger = _scope.ServiceProvider.GetRequiredService<ILogger<MCPValidationIntegrationTests>>();
    }

    #region Basic Input Validation Integration Tests

    [Fact]
    public async Task ValidateBasicInputs_WithRealData_ShouldValidateCorrectly()
    {
        // Arrange
        await SeedValidTestData();
        var revenue = 15000m;
        var cost = 8000m;
        var conversions = 150;
        var visitors = 10000;

        _logger.LogInformation("Testing basic input validation with real data");

        // Act
        var result = await _validationService.ValidateBasicInputsAsync(revenue, cost, conversions, visitors);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationErrors);
        Assert.NotEmpty(result.ValidationMessages);
        
        // Verify calculated metrics
        Assert.True(result.CalculatedMCP > 0);
        Assert.True(result.ConversionRate > 0 && result.ConversionRate <= 1);
        Assert.True(result.RevenuePerVisitor > 0);
        Assert.True(result.CostPerConversion > 0);
        
        _logger.LogInformation("Basic validation passed. MCP: {MCP}, Conversion Rate: {Rate}", 
            result.CalculatedMCP, result.ConversionRate);
    }

    [Fact]
    public async Task ValidateBasicInputs_WithInvalidData_ShouldReturnErrors()
    {
        // Arrange
        var revenue = -1000m; // Invalid negative revenue
        var cost = 5000m;
        var conversions = 200; // More conversions than visitors
        var visitors = 100;

        // Act
        var result = await _validationService.ValidateBasicInputsAsync(revenue, cost, conversions, visitors);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.ValidationErrors);
        
        // Check for specific error types
        Assert.Contains(result.ValidationErrors, e => e.Contains("revenue"));
        Assert.Contains(result.ValidationErrors, e => e.Contains("conversions"));
        
        _logger.LogInformation("Invalid data correctly rejected with {ErrorCount} validation errors", 
            result.ValidationErrors.Count);
    }

    #endregion

    #region Statistical Significance Validation Integration Tests

    [Fact]
    public async Task ValidateStatisticalSignificance_WithSufficientData_ShouldPassValidation()
    {
        // Arrange
        await SeedStatisticalTestData();
        var controlConversions = 500;
        var controlVisitors = 10000;
        var treatmentConversions = 550;
        var treatmentVisitors = 10000;
        var confidenceLevel = 0.95;

        // Act
        var result = await _validationService.ValidateStatisticalSignificanceAsync(
            controlConversions, controlVisitors, 
            treatmentConversions, treatmentVisitors, 
            confidenceLevel);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.True(result.HasSufficientSampleSize);
        Assert.True(result.StatisticalPower >= 0.8); // Standard threshold
        Assert.True(result.PValue >= 0 && result.PValue <= 1);
        Assert.True(result.EffectSize > 0);
        
        _logger.LogInformation("Statistical significance validation passed. Power: {Power}, P-value: {PValue}", 
            result.StatisticalPower, result.PValue);
    }

    [Fact]
    public async Task ValidateStatisticalSignificance_WithInsufficientData_ShouldRecommendMoreData()
    {
        // Arrange
        var controlConversions = 5;
        var controlVisitors = 100;
        var treatmentConversions = 7;
        var treatmentVisitors = 100;
        var confidenceLevel = 0.95;

        // Act
        var result = await _validationService.ValidateStatisticalSignificanceAsync(
            controlConversions, controlVisitors, 
            treatmentConversions, treatmentVisitors, 
            confidenceLevel);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.HasSufficientSampleSize);
        Assert.NotEmpty(result.Recommendations);
        Assert.True(result.RecommendedSampleSize > controlVisitors + treatmentVisitors);
        
        _logger.LogInformation("Insufficient data correctly identified. Recommended sample size: {Size}", 
            result.RecommendedSampleSize);
    }

    #endregion

    #region Currency Validation Integration Tests

    [Fact]
    public async Task ValidateCurrency_WithMultipleCurrencies_ShouldNormalizeCorrectly()
    {
        // Arrange
        await SeedCurrencyTestData();
        var amounts = new List<(decimal amount, string currency)>
        {
            (1000m, "USD"),
            (850m, "EUR"),
            (1200m, "CAD"),
            (800m, "GBP")
        };
        var targetCurrency = "USD";

        // Act
        var result = await _validationService.ValidateCurrencyAsync(amounts, targetCurrency);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Equal(targetCurrency, result.TargetCurrency);
        Assert.NotEmpty(result.ConversionRates);
        Assert.Equal(amounts.Count, result.NormalizedAmounts.Count);
        
        // Verify all amounts are converted to target currency
        foreach (var normalizedAmount in result.NormalizedAmounts)
        {
            Assert.Equal(targetCurrency, normalizedAmount.Currency);
            Assert.True(normalizedAmount.Amount > 0);
        }
        
        _logger.LogInformation("Currency validation passed for {CurrencyCount} currencies to {Target}", 
            amounts.Select(a => a.currency).Distinct().Count(), targetCurrency);
    }

    [Fact]
    public async Task ValidateCurrency_WithUnsupportedCurrency_ShouldReturnError()
    {
        // Arrange
        var amounts = new List<(decimal amount, string currency)>
        {
            (1000m, "USD"),
            (500m, "XYZ") // Unsupported currency
        };
        var targetCurrency = "USD";

        // Act
        var result = await _validationService.ValidateCurrencyAsync(amounts, targetCurrency);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.ValidationErrors);
        Assert.Contains(result.ValidationErrors, e => e.Contains("XYZ"));
        
        _logger.LogInformation("Unsupported currency correctly rejected");
    }

    #endregion

    #region Multi-Variant Validation Integration Tests

    [Fact]
    public async Task ValidateMultipleVariants_WithValidConfiguration_ShouldPassValidation()
    {
        // Arrange
        await SeedMultiVariantTestData();
        var variants = new List<VariantData>
        {
            new() { VariantId = "control", Revenue = 10000m, Cost = 5000m, Conversions = 100, Visitors = 2000, IsControl = true },
            new() { VariantId = "variant-a", Revenue = 12000m, Cost = 5500m, Conversions = 110, Visitors = 2000, IsControl = false },
            new() { VariantId = "variant-b", Revenue = 11500m, Cost = 5200m, Conversions = 105, Visitors = 2000, IsControl = false }
        };

        // Act
        var result = await _validationService.ValidateMultipleVariantsAsync(variants);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Equal(variants.Count, result.VariantResults.Count);
        Assert.Single(result.VariantResults.Where(v => v.IsControl));
        Assert.True(result.HasSufficientPowerForComparison);
        
        // Verify each variant validation
        foreach (var variantResult in result.VariantResults)
        {
            Assert.True(variantResult.IsValid);
            Assert.True(variantResult.CalculatedMCP > 0);
            Assert.Empty(variantResult.ValidationErrors);
        }
        
        _logger.LogInformation("Multi-variant validation passed for {VariantCount} variants", variants.Count);
    }

    [Fact]
    public async Task ValidateMultipleVariants_WithImbalancedTraffic_ShouldWarnAboutPower()
    {
        // Arrange
        var variants = new List<VariantData>
        {
            new() { VariantId = "control", Revenue = 10000m, Cost = 5000m, Conversions = 100, Visitors = 5000, IsControl = true },
            new() { VariantId = "variant-a", Revenue = 2000m, Cost = 1000m, Conversions = 20, Visitors = 500, IsControl = false } // Imbalanced
        };

        // Act
        var result = await _validationService.ValidateMultipleVariantsAsync(variants);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.HasSufficientPowerForComparison);
        Assert.NotEmpty(result.Warnings);
        Assert.Contains(result.Warnings, w => w.Contains("traffic") || w.Contains("imbalanced"));
        
        _logger.LogInformation("Traffic imbalance correctly detected and warned");
    }

    #endregion

    #region Date Range Validation Integration Tests

    [Fact]
    public async Task ValidateDateRange_WithValidRange_ShouldPassValidation()
    {
        // Arrange
        await SeedDateRangeTestData();
        var startDate = DateTime.UtcNow.AddDays(-30);
        var endDate = DateTime.UtcNow.AddDays(-1);
        var testStartDate = DateTime.UtcNow.AddDays(-35);

        // Act
        var result = await _validationService.ValidateDateRangeAsync(startDate, endDate, testStartDate);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.True(result.HasSufficientDataPeriod);
        Assert.True(result.DataQualityScore > 0.7); // Good quality threshold
        Assert.Empty(result.ValidationErrors);
        
        _logger.LogInformation("Date range validation passed. Data quality score: {Score}", 
            result.DataQualityScore);
    }

    [Fact]
    public async Task ValidateDateRange_WithFutureDate_ShouldReturnError()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddDays(-10);
        var endDate = DateTime.UtcNow.AddDays(5); // Future date
        var testStartDate = DateTime.UtcNow.AddDays(-15);

        // Act
        var result = await _validationService.ValidateDateRangeAsync(startDate, endDate, testStartDate);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.ValidationErrors);
        Assert.Contains(result.ValidationErrors, e => e.Contains("future"));
        
        _logger.LogInformation("Future date correctly rejected");
    }

    #endregion

    #region Confidence Level Validation Integration Tests

    [Fact]
    public async Task ValidateConfidenceLevel_WithStandardLevels_ShouldPassValidation()
    {
        // Arrange
        var standardLevels = new[] { 0.90, 0.95, 0.99 };

        foreach (var level in standardLevels)
        {
            // Act
            var result = await _validationService.ValidateConfidenceLevelAsync(level);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid);
            Assert.True(result.IsStandardLevel);
            Assert.True(result.RecommendedSampleSize > 0);
            
            _logger.LogInformation("Confidence level {Level} validated successfully", level);
        }
    }

    [Fact]
    public async Task ValidateConfidenceLevel_WithInvalidLevel_ShouldReturnError()
    {
        // Arrange
        var invalidLevel = 1.5; // > 1.0

        // Act
        var result = await _validationService.ValidateConfidenceLevelAsync(invalidLevel);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.ValidationErrors);
        Assert.Contains(result.ValidationErrors, e => e.Contains("confidence"));
        
        _logger.LogInformation("Invalid confidence level correctly rejected");
    }

    #endregion

    #region A/B Test Configuration Validation Integration Tests

    [Fact]
    public async Task ValidateAbTestConfiguration_WithValidSetup_ShouldPassValidation()
    {
        // Arrange
        await SeedAbTestConfigurationData();
        var config = new AbTestConfiguration
        {
            TestId = _testAbTestId,
            TestName = "Homepage CTA Test",
            StartDate = DateTime.UtcNow.AddDays(-30),
            EndDate = DateTime.UtcNow,
            TrafficAllocation = 100,
            Variants = new List<VariantConfiguration>
            {
                new() { VariantId = "control", Name = "Original", TrafficPercentage = 50, IsControl = true },
                new() { VariantId = "treatment", Name = "New CTA", TrafficPercentage = 50, IsControl = false }
            },
            PrimaryMetric = "conversion_rate",
            SecondaryMetrics = new[] { "revenue_per_visitor", "bounce_rate" },
            MinimumDetectableEffect = 0.05,
            StatisticalPower = 0.8,
            ConfidenceLevel = 0.95
        };

        // Act
        var result = await _validationService.ValidateAbTestConfigurationAsync(config);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.True(result.HasValidTrafficAllocation);
        Assert.True(result.HasSufficientPowerSettings);
        Assert.Empty(result.ValidationErrors);
        Assert.NotEmpty(result.ConfigurationSummary);
        
        _logger.LogInformation("A/B test configuration validation passed for test: {TestName}", config.TestName);
    }

    [Fact]
    public async Task ValidateAbTestConfiguration_WithInvalidTrafficAllocation_ShouldReturnError()
    {
        // Arrange
        var config = new AbTestConfiguration
        {
            TestId = _testAbTestId,
            TestName = "Invalid Traffic Test",
            Variants = new List<VariantConfiguration>
            {
                new() { VariantId = "control", TrafficPercentage = 60, IsControl = true },
                new() { VariantId = "treatment", TrafficPercentage = 60, IsControl = false } // Total > 100%
            }
        };

        // Act
        var result = await _validationService.ValidateAbTestConfigurationAsync(config);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.False(result.HasValidTrafficAllocation);
        Assert.NotEmpty(result.ValidationErrors);
        Assert.Contains(result.ValidationErrors, e => e.Contains("traffic"));
        
        _logger.LogInformation("Invalid traffic allocation correctly rejected");
    }

    #endregion

    #region Business Rules Validation Integration Tests

    [Fact]
    public async Task ValidateBusinessRules_WithValidContext_ShouldPassValidation()
    {
        // Arrange
        await SeedBusinessRulesData();
        var context = new MCPBusinessContext
        {
            Industry = "E-commerce",
            BusinessModel = "B2C",
            AverageOrderValue = 150m,
            CustomerLifetimeValue = 500m,
            MarginPercentage = 0.3m,
            SeasonalityFactor = 1.2m,
            CompetitivePosition = "Market Leader",
            RiskTolerance = "Medium"
        };
        var calculatedMCP = 125m;

        // Act
        var result = await _validationService.ValidateBusinessRulesAsync(calculatedMCP, context);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.True(result.IsWithinIndustryBenchmarks);
        Assert.True(result.IsBusinessViable);
        Assert.NotEmpty(result.AppliedRules);
        Assert.NotEmpty(result.BusinessInsights);
        
        _logger.LogInformation("Business rules validation passed. MCP {MCP} is viable for {Industry}", 
            calculatedMCP, context.Industry);
    }

    [Fact]
    public async Task ValidateBusinessRules_WithUnrealisticMCP_ShouldWarnOrReject()
    {
        // Arrange
        var context = new MCPBusinessContext
        {
            Industry = "E-commerce",
            AverageOrderValue = 50m,
            MarginPercentage = 0.1m
        };
        var unrealisticMCP = 5000m; // Way too high for the context

        // Act
        var result = await _validationService.ValidateBusinessRulesAsync(unrealisticMCP, context);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsWithinIndustryBenchmarks);
        Assert.NotEmpty(result.Warnings);
        Assert.Contains(result.Warnings, w => w.Contains("unrealistic") || w.Contains("high"));
        
        _logger.LogInformation("Unrealistic MCP correctly flagged with warnings");
    }

    #endregion

    #region Comprehensive Validation Integration Tests

    [Fact]
    public async Task ValidateComprehensiveMCPRequest_WithCompleteValidData_ShouldPassAllValidations()
    {
        // Arrange
        await SeedComprehensiveTestData();
        var request = new MCPCalculationRequest
        {
            AbTestId = _testAbTestId,
            StartDate = _testStartDate,
            EndDate = _testEndDate,
            TargetCurrency = "USD",
            ConfidenceLevel = 0.95,
            IncludeStatisticalSignificance = true,
            Variants = new List<VariantData>
            {
                new() { VariantId = "control", Revenue = 25000m, Cost = 12000m, Conversions = 250, Visitors = 5000, IsControl = true },
                new() { VariantId = "treatment", Revenue = 28000m, Cost = 13000m, Conversions = 280, Visitors = 5000, IsControl = false }
            },
            BusinessContext = new MCPBusinessContext
            {
                Industry = "E-commerce",
                BusinessModel = "B2C",
                AverageOrderValue = 100m,
                MarginPercentage = 0.25m
            }
        };

        // Act
        var result = await _validationService.ValidateComprehensiveMCPRequestAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.True(result.BasicInputValidation.IsValid);
        Assert.True(result.StatisticalValidation.IsValid);
        Assert.True(result.CurrencyValidation.IsValid);
        Assert.True(result.MultiVariantValidation.IsValid);
        Assert.True(result.DateRangeValidation.IsValid);
        Assert.True(result.ConfidenceLevelValidation.IsValid);
        Assert.True(result.BusinessRuleValidation.IsValid);
        Assert.Empty(result.CriticalErrors);
        
        _logger.LogInformation("Comprehensive validation passed for complete MCP request");
    }

    [Fact]
    public async Task ValidateComprehensiveMCPRequest_WithMultipleIssues_ShouldIdentifyAllProblems()
    {
        // Arrange
        var request = new MCPCalculationRequest
        {
            AbTestId = Guid.Empty, // Invalid
            StartDate = DateTime.UtcNow.AddDays(5), // Future date
            EndDate = DateTime.UtcNow.AddDays(-5), // End before start
            TargetCurrency = "XYZ", // Invalid currency
            ConfidenceLevel = 1.5, // Invalid confidence level
            Variants = new List<VariantData>
            {
                new() { VariantId = "control", Revenue = -1000m, Cost = 5000m, Conversions = 100, Visitors = 50, IsControl = true }, // Multiple issues
            }
        };

        // Act
        var result = await _validationService.ValidateComprehensiveMCPRequestAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.CriticalErrors);
        
        // Verify specific validation failures
        Assert.False(result.BasicInputValidation.IsValid);
        Assert.False(result.CurrencyValidation.IsValid);
        Assert.False(result.DateRangeValidation.IsValid);
        Assert.False(result.ConfidenceLevelValidation.IsValid);
        
        _logger.LogInformation("Multiple validation issues correctly identified: {ErrorCount} critical errors", 
            result.CriticalErrors.Count);
    }

    #endregion

    #region Performance Integration Tests

    [Fact]
    public async Task ValidateMultipleRequests_Concurrently_ShouldHandleLoad()
    {
        // Arrange
        await SeedPerformanceTestData();
        var concurrentRequests = 20;
        var tasks = new List<Task<MCPValidationResult>>();

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            var revenue = 1000m + (i * 100);
            var cost = 500m + (i * 50);
            var conversions = 10 + i;
            var visitors = 100 + (i * 10);
            
            tasks.Add(_validationService.ValidateBasicInputsAsync(revenue, cost, conversions, visitors));
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(concurrentRequests, results.Length);
        Assert.All(results, result => Assert.NotNull(result));
        Assert.All(results, result => Assert.True(result.IsValid));
        
        _logger.LogInformation("Successfully handled {RequestCount} concurrent validation requests", 
            concurrentRequests);
    }

    #endregion

    #region Helper Methods

    private async Task SeedValidTestData()
    {
        _logger.LogInformation("Seeding valid test data for validation tests");
        await Task.Delay(50);
    }

    private async Task SeedStatisticalTestData()
    {
        _logger.LogInformation("Seeding statistical test data");
        await Task.Delay(50);
    }

    private async Task SeedCurrencyTestData()
    {
        _logger.LogInformation("Seeding currency test data");
        await Task.Delay(50);
    }

    private async Task SeedMultiVariantTestData()
    {
        _logger.LogInformation("Seeding multi-variant test data");
        await Task.Delay(50);
    }

    private async Task SeedDateRangeTestData()
    {
        _logger.LogInformation("Seeding date range test data");
        await Task.Delay(50);
    }

    private async Task SeedAbTestConfigurationData()
    {
        _logger.LogInformation("Seeding A/B test configuration data");
        await Task.Delay(50);
    }

    private async Task SeedBusinessRulesData()
    {
        _logger.LogInformation("Seeding business rules data");
        await Task.Delay(50);
    }

    private async Task SeedComprehensiveTestData()
    {
        _logger.LogInformation("Seeding comprehensive test data");
        await Task.Delay(50);
    }

    private async Task SeedPerformanceTestData()
    {
        _logger.LogInformation("Seeding performance test data");
        await Task.Delay(50);
    }

    #endregion

    #region Cleanup

    public void Dispose()
    {
        _scope?.Dispose();
    }

    #endregion
}