using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Service for MCP analytics and calculations
/// Provides comprehensive MCP analysis functionality
/// </summary>
public class MCPAnalyticsService : IMCPAnalyticsService
{
    private readonly IRevenueRepository _revenueRepository;
    private readonly IAbTestRepository _abTestRepository;
    private readonly IVariantRepository _variantRepository;
    private readonly IConversionRepository _conversionRepository;
    private readonly ILogger<MCPAnalyticsService> _logger;
    private readonly IProfitTrackingService _profitTrackingService;
    private readonly ICurrencyConversionService _currencyConversionService;

    public MCPAnalyticsService(
        IRevenueRepository revenueRepository,
        IAbTestRepository abTestRepository,
        IVariantRepository variantRepository,
        IConversionRepository conversionRepository,
        ILogger<MCPAnalyticsService> logger,
        IProfitTrackingService profitTrackingService,
        ICurrencyConversionService currencyConversionService
        )
    {
        _revenueRepository = revenueRepository;
        _abTestRepository = abTestRepository;
        _variantRepository = variantRepository;
        _conversionRepository = conversionRepository;
        _logger = logger;
        _profitTrackingService = profitTrackingService;
        _currencyConversionService = currencyConversionService;
    }
    public async Task<decimal> CalculateMCPAsync(decimal revenue, decimal cost, int conversions, int visitors)
    {
        try
        {
            _logger.LogInformation("Calculating MCP for revenue: {Revenue}, cost: {Cost}, conversions: {Conversions}, visitors: {Visitors}",
                revenue, cost, conversions, visitors);
            if (conversions <= 0)
            {
                _logger.LogWarning("No conversions found, returning 0 MCP");
                return 0m;
            }
            var profit = revenue - cost;
            var mcp = profit / conversions;
            _logger.LogInformation("Calculated MCP: {MCP}", mcp);
            return Math.Round(mcp, 2);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP");
            throw;
        }
    }
    public Task<MCPInputValidationResult> ValidateMCPInputs(decimal minMcp, int minMcpSampleSize, decimal maxMcp, int maxMcpSampleSize)
    {
        // Basic validation for now, can be expanded later
        var errors = new List<string>();
        var warnings = new List<string>();

        if (minMcp < 0) errors.Add("Minimum MCP cannot be negative.");
        if (maxMcp < 0) errors.Add("Maximum MCP cannot be negative.");
        if (minMcpSampleSize <= 0) errors.Add("Minimum sample size for MCP cannot be zero or negative.");
        if (maxMcpSampleSize <= 0) errors.Add("Maximum sample size for MCP cannot be zero or negative.");

        return Task.FromResult(new MCPInputValidationResult
        {
            IsValid = !errors.Any(),
            ErrorMessages = errors,
            ValidationMessages = warnings
        });
    }    

    public async Task<decimal> NormalizeCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
    {
        try
        {
            _logger.LogInformation("Normalizing currency from {FromCurrency} to {ToCurrency}", fromCurrency, toCurrency);
            var exchangeRate = await _currencyConversionService.GetConversionRateAsync(fromCurrency, toCurrency);
            var normalizedAmount = amount * exchangeRate;
            _logger.LogInformation("Normalized amount: {NormalizedAmount}", normalizedAmount);
            return normalizedAmount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error normalizing currency");
            throw;
        }
    }

    public async Task<decimal> CalculateMCPWithCurrencyAsync(decimal revenue, decimal cost, int conversions, int visitors, string currency)
    {
        try
        {
            _logger.LogInformation("Calculating MCP with currency normalization for currency: {Currency}", currency);

            // For MVP, we'll assume USD as base currency
            // In production, this would integrate with a currency conversion service
            var normalizedRevenue = await NormalizeCurrencyAsync(revenue, currency, "USD");
            var normalizedCost = await NormalizeCurrencyAsync(cost, currency, "USD");

            return await CalculateMCPAsync(normalizedRevenue, normalizedCost, conversions, visitors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP with currency normalization");
            throw;
        }
    }

    public async Task<List<MCPVariantResult>> CalculateMCPForMultipleVariantsAsync(List<VariantData> variants)
    {
        try
        {
            _logger.LogInformation("Calculating MCP for {VariantCount} variants", variants.Count);

            var results = new List<MCPVariantResult>();

            foreach (var variant in variants)
            {
                var mcp = await CalculateMCPAsync(variant.Revenue, variant.Cost, variant.Conversions, variant.Visitors);
                var conversionRate = variant.Visitors > 0 ? (decimal)variant.Conversions / variant.Visitors : 0;
                var revenuePerVisitor = variant.Visitors > 0 ? variant.Revenue / variant.Visitors : 0;

                results.Add(new MCPVariantResult
                {
                    VariantId = variant.VariantId,
                    MCP = mcp,
                    Revenue = variant.Revenue,
                    Cost = variant.Cost,
                    Conversions = variant.Conversions,
                    Visitors = variant.Visitors,
                    ConversionRate = conversionRate,
                    RevenuePerVisitor = revenuePerVisitor,
                    IsControl = variant.IsControl,
                    CalculatedAt = DateTime.UtcNow
                });
            }

            _logger.LogInformation("Calculated MCP for all variants successfully");
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP for multiple variants");
            throw;
        }
    }

    public async Task<MCPStatisticalResult> CalculateMCPWithStatisticalSignificanceAsync(
        decimal controlRevenue, decimal controlCost, int controlConversions, int controlVisitors,
        decimal treatmentRevenue, decimal treatmentCost, int treatmentConversions, int treatmentVisitors,
        double confidenceLevel = 0.95)
    {
        try
        {
            _logger.LogInformation("Calculating MCP with statistical significance analysis");

            var controlMCP = await CalculateMCPAsync(controlRevenue, controlCost, controlConversions, controlVisitors);
            var treatmentMCP = await CalculateMCPAsync(treatmentRevenue, treatmentCost, treatmentConversions, treatmentVisitors);

            // Calculate statistical significance
            var controlConversionRate = controlVisitors > 0 ? (double)controlConversions / controlVisitors : 0;
            var treatmentConversionRate = treatmentVisitors > 0 ? (double)treatmentConversions / treatmentVisitors : 0;

            var pooledConversionRate = (controlConversions + treatmentConversions) / (double)(controlVisitors + treatmentVisitors);
            var standardError = Math.Sqrt(pooledConversionRate * (1 - pooledConversionRate) * (1.0 / controlVisitors + 1.0 / treatmentVisitors));
            
            var zScore = standardError > 0 ? (treatmentConversionRate - controlConversionRate) / standardError : 0;
            var pValue = 2 * (1 - NormalCDF(Math.Abs(zScore)));
            
            var isSignificant = pValue < (1 - confidenceLevel);
            var improvement = controlMCP != 0 ? (double)(((treatmentMCP - controlMCP) / controlMCP) * 100) : 0;

            return new MCPStatisticalResult
            {
                ControlMCP = controlMCP,
                TreatmentMCP = treatmentMCP,
                MCPImprovement = improvement,
                IsStatisticallySignificant = isSignificant,
                PValue = pValue,
                ConfidenceLevel = (double)confidenceLevel,
                ZScore = zScore,
                SampleSize = controlVisitors + treatmentVisitors,
                CalculatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP with statistical significance");
            throw;
        }
    }

    public async Task<MCPAnalysisReport> GenerateMCPAnalysisReportAsync(Guid abTestId, DateTime startDate, DateTime endDate)
    {
        try
        {
            _logger.LogInformation("Generating MCP analysis report for A/B test {AbTestId}", abTestId);

            var abTest = await _abTestRepository.GetByIdAsync(abTestId);
            if (abTest == null)
            {
                throw new ArgumentException($"A/B test with ID {abTestId} not found");
            }

            var variants = await _variantRepository.GetByAbTestIdAsync(abTestId);
            var revenues = await _revenueRepository.GetByAbTestIdAndDateRangeAsync(abTestId, startDate, endDate);
            var conversions = await _conversionRepository.GetByAbTestIdAndDateRangeAsync(abTestId, startDate, endDate);

            // Calculate overall metrics
            var totalRevenue = revenues.Sum(r => r.Amount);
            var totalCost = variants.Sum(v => v.Cost);
            var totalConversions = conversions.Count();
            var totalVisitors = variants.Sum(v => v.Visitors);

            var overallMCP = await CalculateMCPAsync(totalRevenue, totalCost, totalConversions, totalVisitors);

            // Calculate variant-specific results
            var variantResults = new List<VariantComparison>();
            foreach (var variant in variants)
            {
                var variantRevenues = revenues.Where(r => r.VariantId == variant.Id).Sum(r => r.Amount);
                var variantConversions = conversions.Count(c => c.VariantId == variant.Id);
                var variantMCP = await CalculateMCPAsync(variantRevenues, variant.Cost, variantConversions, variant.Visitors);

                variantResults.Add(new VariantComparison
                {
                    VariantId = variant.Id.ToString(),
                    VariantName = variant.Name,
                    MCP = variantMCP,
                    Revenue = variantRevenues,
                    Conversions = variantConversions,
                    Visitors = variant.Visitors,
                    ConversionRate = variant.Visitors > 0 ? (double)variantConversions / variant.Visitors : 0,
                    IsControl = variant.IsControl
                });
            }

            // Generate statistical insights
            var insights = new MCPStatisticalInsights
            {
                SampleSize = totalVisitors,
                ConfidenceLevel = 0.95,
                StatisticalPower = CalculateStatisticalPower(variantResults),
                RecommendedSampleSize = CalculateRecommendedSampleSize(variantResults)
            };

            return new MCPAnalysisReport
            {
                AbTestId = abTestId,
                ReportGeneratedAt = DateTime.UtcNow,
                AnalysisPeriod = new DateRange { StartDate = startDate, EndDate = endDate },
                OverallMCP = overallMCP,
                TotalRevenue = totalRevenue,
                TotalConversions = totalConversions,
                TotalVisitors = totalVisitors,
                VariantResults = variantResults,
                StatisticalInsights = insights
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating MCP analysis report");
            throw;
        }
    }

    public async Task<List<MCPTrendPoint>> GetMCPTrendsAsync(Guid abTestId, DateTime startDate, DateTime endDate, string granularity = "daily")
    {
        try
        {
            _logger.LogInformation("Getting MCP trends for A/B test {AbTestId} with {Granularity} granularity", abTestId, granularity);

            var revenues = await _revenueRepository.GetByAbTestIdAndDateRangeAsync(abTestId, startDate, endDate);
            var conversions = await _conversionRepository.GetByAbTestIdAndDateRangeAsync(abTestId, startDate, endDate);
            var variants = await _variantRepository.GetByAbTestIdAsync(abTestId);

            var trendPoints = new List<MCPTrendPoint>();
            var currentDate = startDate.Date;

            while (currentDate <= endDate.Date)
            {
                var nextDate = granularity.ToLower() switch
                {
                    "hourly" => currentDate.AddHours(1),
                    "daily" => currentDate.AddDays(1),
                    "weekly" => currentDate.AddDays(7),
                    "monthly" => currentDate.AddMonths(1),
                    _ => currentDate.AddDays(1)
                };

                var periodRevenues = revenues.Where(r => r.CreatedAt >= currentDate && r.CreatedAt < nextDate).Sum(r => r.Amount);
                var periodConversions = conversions.Count(c => c.CreatedAt >= currentDate && c.CreatedAt < nextDate);
                var periodCost = variants.Sum(v => v.Cost / ((endDate - startDate).Days + 1)); // Distribute cost evenly

                var periodMCP = periodConversions > 0 ? (periodRevenues - periodCost) / periodConversions : 0;

                trendPoints.Add(new MCPTrendPoint
                {
                    Date = currentDate,
                    MCP = periodMCP,
                    Revenue = periodRevenues,
                    Conversions = periodConversions,
                    Visitors = 0 // Would need visitor tracking by period
                });

                currentDate = nextDate;
            }

            _logger.LogInformation("Generated {TrendPointCount} trend points", trendPoints.Count);
            return trendPoints;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting MCP trends");
            throw;
        }
    }

    public async Task<MCPInputValidationResult> ValidateMCPInputsAsync(decimal revenue, decimal cost, int conversions, int visitors)
    {
        try
        {
            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic validation
            if (revenue < 0)
                validationErrors.Add("Revenue cannot be negative");
            if (cost < 0)
                validationErrors.Add("Cost cannot be negative");
            if (conversions < 0)
                validationErrors.Add("Conversions cannot be negative");
            if (visitors < 0)
                validationErrors.Add("Visitors cannot be negative");
            if (conversions > visitors)
                validationErrors.Add("Conversions cannot exceed visitors");

            // Business logic validation
            if (conversions == 0)
                validationMessages.Add("No conversions detected - MCP will be 0");
            if (visitors < 100)
                validationMessages.Add("Low visitor count may affect statistical reliability");
            if (cost > revenue)
                validationMessages.Add("Cost exceeds revenue - negative profit margin");

            var isValid = !validationErrors.Any();
            var calculatedMCP = isValid && conversions > 0 ? (revenue - cost) / conversions : 0;
            var conversionRate = visitors > 0 ? (decimal)conversions / visitors : 0;

            return new MCPInputValidationResult
            {
                IsValid = isValid,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                CalculatedMCP = calculatedMCP,
                ConversionRate = conversionRate,
                RevenuePerVisitor = visitors > 0 ? revenue / visitors : 0,
                CostPerConversion = conversions > 0 ? cost / conversions : 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating MCP inputs");
            throw;
        }
    }

    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalAsync(
        decimal revenue, decimal cost, int conversions, int visitors, double confidenceLevel = 0.95)
    {
        try
        {
            _logger.LogInformation("Calculating MCP with confidence interval at {ConfidenceLevel} confidence level", confidenceLevel);

            var mcp = await CalculateMCPAsync(revenue, cost, conversions, visitors);
            
            if (conversions == 0 || visitors == 0)
            {
                return new MCPConfidenceInterval
                {
                    MCP = mcp,
                    LowerBound = 0,
                    UpperBound = 0,
                    ConfidenceLevel = confidenceLevel,
                    MarginOfError = 0
                };
            }

            // Calculate standard error for MCP
            var conversionRate = (double)conversions / visitors;
            var revenuePerConversion = conversions > 0 ? revenue / conversions : 0;
            var costPerConversion = conversions > 0 ? cost / conversions : 0;
            
            // Simplified confidence interval calculation
            // In production, this would use more sophisticated statistical methods
            var standardError = Math.Sqrt(conversionRate * (1 - conversionRate) / visitors) * (double)revenuePerConversion;
            var zScore = GetZScoreForConfidenceLevel(confidenceLevel);
            var marginOfError = zScore * standardError;

            return new MCPConfidenceInterval
            {
                MCP = mcp,
                LowerBound = Math.Max(0, mcp - (decimal)marginOfError),
                UpperBound = mcp + (decimal)marginOfError,
                ConfidenceLevel = confidenceLevel,
                MarginOfError = marginOfError
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP with confidence interval");
            throw;
        }
    }

    #region Private Helper Methods



    private static double NormalCDF(double x)
    {
        // Approximation of the cumulative distribution function for standard normal distribution
        return 0.5 * (1 + Erf(x / Math.Sqrt(2)));
    }

    private static double Erf(double x)
    {
        // Approximation of the error function
        const double a1 = 0.254829592;
        const double a2 = -0.284496736;
        const double a3 = 1.421413741;
        const double a4 = -1.453152027;
        const double a5 = 1.061405429;
        const double p = 0.3275911;

        var sign = x < 0 ? -1 : 1;
        x = Math.Abs(x);

        var t = 1.0 / (1.0 + p * x);
        var y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

        return sign * y;
    }

    private static double CalculateStatisticalPower(List<VariantComparison> variants)
    {
        // Simplified statistical power calculation
        // In production, this would use more sophisticated methods
        if (variants.Count < 2) return 0;

        var totalSampleSize = variants.Sum(v => v.Visitors);
        return Math.Min(0.99, totalSampleSize / 10000.0); // Simplified formula
    }

    private static int CalculateRecommendedSampleSize(List<VariantComparison> variants)
    {
        // Simplified sample size calculation
        // In production, this would consider effect size, power, and significance level
        var currentSampleSize = variants.Sum(v => v.Visitors);
        var recommendedMinimum = 1000 * variants.Count; // 1000 per variant minimum
        
        return Math.Max(recommendedMinimum, currentSampleSize * 2);
    }

    private static double GetZScoreForConfidenceLevel(double confidenceLevel)
    {
        // Common z-scores for confidence levels
        return confidenceLevel switch
        {
            0.90 => 1.645,
            0.95 => 1.96,
            0.99 => 2.576,
            _ => 1.96 // Default to 95%
        };
    }

    public decimal CalculateMCP(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
    {
        throw new NotImplementedException();
    }

    public decimal CalculateMCPWithCurrency(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, string currency)
    {
        throw new NotImplementedException();
    }

    public List<MCPResult> CalculateMCPForMultipleVariants(List<VariantData> variants)
    {
        throw new NotImplementedException();
    }

    public decimal? CalculateMCPWithSignificance(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
    {
        throw new NotImplementedException();
    }

    public async Task<MCPAnalysisReport> GenerateMCPAnalysisAsync(Guid abTestId, DateTime startDate, DateTime endDate)
    {
        return await GenerateMCPAnalysisReportAsync(abTestId, startDate, endDate);
    }

    public MCPConfidenceInterval CalculateMCPWithConfidence(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, double confidenceLevel = 0.95)
    {
        try
        {
            // Calculate MCP for control and variant
            var controlMCP = controlConversions > 0 ? controlRevenue / controlConversions : 0;
            var variantMCP = variantConversions > 0 ? variantRevenue / variantConversions : 0;
            var mcpDifference = variantMCP - controlMCP;

            if (controlConversions == 0 || variantConversions == 0)
            {
                return new MCPConfidenceInterval
                {
                    MCP = mcpDifference,
                    LowerBound = 0,
                    UpperBound = 0,
                    ConfidenceLevel = confidenceLevel,
                    MarginOfError = 0
                };
            }

            // Simplified confidence interval calculation
            var controlVariance = Math.Pow((double)controlRevenue / controlConversions, 2) / controlConversions;
            var variantVariance = Math.Pow((double)variantRevenue / variantConversions, 2) / variantConversions;
            var combinedStandardError = Math.Sqrt(controlVariance + variantVariance);

            var zScore = GetZScoreForConfidenceLevel(confidenceLevel);
            var marginOfError = zScore * combinedStandardError;

            return new MCPConfidenceInterval
            {
                MCP = mcpDifference,
                LowerBound = mcpDifference - (decimal)marginOfError,
                UpperBound = mcpDifference + (decimal)marginOfError,
                ConfidenceLevel = confidenceLevel,
                MarginOfError = marginOfError
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP with confidence for control and variant");
            throw;
        }
    }

    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(Guid abTestId, DateTime startDate, DateTime endDate, double confidenceLevel = 0.95)
    {
        try
        {
            _logger.LogInformation("Calculating MCP with confidence intervals for A/B test {AbTestId} from {StartDate} to {EndDate}", abTestId, startDate, endDate);

            // Get A/B test data
            var abTest = await _abTestRepository.GetByIdAsync(abTestId);
            if (abTest == null)
            {
                throw new ArgumentException($"A/B test with ID {abTestId} not found");
            }

            // Get variants for the A/B test
            var variants = (await _variantRepository.GetByAbTestIdAsync(abTestId)).ToList();
            if (variants.Count < 2)
            {
                throw new InvalidOperationException("A/B test must have at least 2 variants for confidence interval calculation");
            }

            // Get revenue and conversion data within date range
            var revenues = await _revenueRepository.GetByAbTestIdAsync(abTestId);
            var conversions = await _conversionRepository.GetByAbTestIdAndDateRangeAsync(abTestId, startDate, endDate);

            // Filter revenue data by date range
            revenues = revenues.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).ToList();

            // Find control and treatment variants
            var controlVariant = variants.FirstOrDefault(v => v.IsControl);
            var treatmentVariant = variants.FirstOrDefault(v => !v.IsControl);

            if (controlVariant == null || treatmentVariant == null)
            {
                throw new InvalidOperationException("A/B test must have both control and treatment variants");
            }

            // Calculate revenue for each variant
            var controlRevenue = revenues.Where(r => r.VariantId == controlVariant.Id).Sum(r => r.Amount);
            var treatmentRevenue = revenues.Where(r => r.VariantId == treatmentVariant.Id).Sum(r => r.Amount);

            // Calculate conversions for each variant
            var controlConversions = conversions.Count(c => c.VariantId == controlVariant.Id);
            var treatmentConversions = conversions.Count(c => c.VariantId == treatmentVariant.Id);

            // Use the existing method to calculate confidence intervals
            return CalculateMCPWithConfidence(controlRevenue, controlConversions, treatmentRevenue, treatmentConversions, confidenceLevel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP with confidence intervals for A/B test {AbTestId} from {StartDate} to {EndDate}", abTestId, startDate, endDate);
            throw;
        }
    }

    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(Guid abTestId, double confidenceLevel = 0.95)
    {
        try
        {
            _logger.LogInformation("Calculating MCP with confidence intervals for A/B test {AbTestId}", abTestId);

            // Get A/B test data
            var abTest = await _abTestRepository.GetByIdAsync(abTestId);
            if (abTest == null)
            {
                throw new ArgumentException($"A/B test with ID {abTestId} not found");
            }

            // Get variants for the A/B test
            var variants = (await _variantRepository.GetByAbTestIdAsync(abTestId)).ToList();
            if (variants.Count < 2)
            {
                throw new InvalidOperationException("A/B test must have at least 2 variants for confidence interval calculation");
            }

            // Get revenue and conversion data
            var revenues = await _revenueRepository.GetByAbTestIdAsync(abTestId);
            var conversions = await _conversionRepository.GetByAbTestIdAndDateRangeAsync(abTestId, DateTime.MinValue, DateTime.MaxValue);

            // Find control and treatment variants
            var controlVariant = variants.FirstOrDefault(v => v.IsControl);
            var treatmentVariant = variants.FirstOrDefault(v => !v.IsControl);

            if (controlVariant == null || treatmentVariant == null)
            {
                throw new InvalidOperationException("A/B test must have both control and treatment variants");
            }

            // Calculate revenue for each variant
            var controlRevenue = revenues.Where(r => r.VariantId == controlVariant.Id).Sum(r => r.Amount);
            var treatmentRevenue = revenues.Where(r => r.VariantId == treatmentVariant.Id).Sum(r => r.Amount);

            // Calculate conversions for each variant
            var controlConversions = conversions.Count(c => c.VariantId == controlVariant.Id);
            var treatmentConversions = conversions.Count(c => c.VariantId == treatmentVariant.Id);

            // Calculate MCP for each variant
            var controlMCP = await CalculateMCPAsync(controlRevenue, controlVariant.Cost, controlConversions, controlVariant.Visitors);
            var treatmentMCP = await CalculateMCPAsync(treatmentRevenue, treatmentVariant.Cost, treatmentConversions, treatmentVariant.Visitors);

            // Calculate the difference in MCP
            var mcpDifference = treatmentMCP - controlMCP;

            // Calculate confidence interval for the MCP difference
            if (controlConversions == 0 || treatmentConversions == 0 || controlVariant.Visitors == 0 || treatmentVariant.Visitors == 0)
            {
                return new MCPConfidenceInterval
                {
                    MCP = mcpDifference,
                    LowerBound = 0,
                    UpperBound = 0,
                    ConfidenceLevel = confidenceLevel,
                    MarginOfError = 0
                };
            }

            // Calculate standard error for the difference in MCP
            var controlConversionRate = (double)controlConversions / controlVariant.Visitors;
            var treatmentConversionRate = (double)treatmentConversions / treatmentVariant.Visitors;
            
            var controlRevenuePerConversion = controlConversions > 0 ? controlRevenue / controlConversions : 0;
            var treatmentRevenuePerConversion = treatmentConversions > 0 ? treatmentRevenue / treatmentConversions : 0;
            
            // Simplified standard error calculation for MCP difference
            var controlStandardError = Math.Sqrt(controlConversionRate * (1 - controlConversionRate) / controlVariant.Visitors) * (double)controlRevenuePerConversion;
            var treatmentStandardError = Math.Sqrt(treatmentConversionRate * (1 - treatmentConversionRate) / treatmentVariant.Visitors) * (double)treatmentRevenuePerConversion;
            var combinedStandardError = Math.Sqrt(Math.Pow(controlStandardError, 2) + Math.Pow(treatmentStandardError, 2));

            var zScore = GetZScoreForConfidenceLevel(confidenceLevel);
            var marginOfError = zScore * combinedStandardError;

            return new MCPConfidenceInterval
            {
                MCP = mcpDifference,
                LowerBound = mcpDifference - (decimal)marginOfError,
                UpperBound = mcpDifference + (decimal)marginOfError,
                ConfidenceLevel = confidenceLevel,
                MarginOfError = marginOfError
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP with confidence intervals for A/B test {AbTestId}", abTestId);
            throw;
        }
    }

    #endregion
}