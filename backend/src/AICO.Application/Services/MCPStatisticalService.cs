using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;
using AICO.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Service for MCP statistical calculations
/// Handles statistical significance, confidence intervals, and power analysis
/// </summary>
public class MCPStatisticalService : IMCPStatisticalService
{
    private readonly IAbTestRepository _abTestRepository;
    private readonly IVariantRepository _variantRepository;
    private readonly IConversionRepository _conversionRepository;
    private readonly IRevenueRepository _revenueRepository;
    private readonly IMCPAnalyticsService _mcpAnalyticsService;
    private readonly ILogger<MCPStatisticalService> _logger;

    public MCPStatisticalService(
        IAbTestRepository abTestRepository,
        IVariantRepository variantRepository,
        IConversionRepository conversionRepository,
        IRevenueRepository revenueRepository,
        IMCPAnalyticsService mcpAnalyticsService,
        ILogger<MCPStatisticalService> logger)
    {
        _abTestRepository = abTestRepository;
        _variantRepository = variantRepository;
        _conversionRepository = conversionRepository;
        _revenueRepository = revenueRepository;
        _mcpAnalyticsService = mcpAnalyticsService;
        _logger = logger;
    }

    public async Task<MCPStatisticalResult> CalculateMCPWithStatisticalSignificanceAsync(
        decimal controlRevenue, decimal controlCost, int controlConversions, int controlVisitors,
        decimal treatmentRevenue, decimal treatmentCost, int treatmentConversions, int treatmentVisitors,
        double confidenceLevel = 0.95)
    {
        try
        {
            _logger.LogInformation("Calculating MCP with statistical significance analysis");

            var controlMCP = await _mcpAnalyticsService.CalculateMCPAsync(controlRevenue, controlCost, controlConversions, controlVisitors);
            var treatmentMCP = await _mcpAnalyticsService.CalculateMCPAsync(treatmentRevenue, treatmentCost, treatmentConversions, treatmentVisitors);

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

    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalAsync(
        decimal revenue, decimal cost, int conversions, int visitors, double confidenceLevel = 0.95)
    {
        try
        {
            _logger.LogInformation("Calculating MCP with confidence interval at {ConfidenceLevel} confidence level", confidenceLevel);

            var mcp = await _mcpAnalyticsService.CalculateMCPAsync(revenue, cost, conversions, visitors);
            
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
            
            // Simplified confidence interval calculation
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

    public MCPConfidenceInterval CalculateMCPWithConfidence(
        decimal controlRevenue, int controlConversions, 
        decimal variantRevenue, int variantConversions, 
        double confidenceLevel = 0.95)
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

    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(
        Guid abTestId, DateTime startDate, DateTime endDate, double confidenceLevel = 0.95)
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

    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(
        Guid abTestId, double confidenceLevel = 0.95)
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
            var controlMCP = await _mcpAnalyticsService.CalculateMCPAsync(controlRevenue, controlVariant.Cost, controlConversions, controlVariant.Visitors);
            var treatmentMCP = await _mcpAnalyticsService.CalculateMCPAsync(treatmentRevenue, treatmentVariant.Cost, treatmentConversions, treatmentVariant.Visitors);

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

    public double CalculateStatisticalPower(List<VariantComparison> variants)
    {
        // Simplified statistical power calculation
        if (variants.Count < 2) return 0;

        var totalSampleSize = variants.Sum(v => v.Visitors);
        return Math.Min(0.99, totalSampleSize / 10000.0); // Simplified formula
    }

    public int CalculateRecommendedSampleSize(List<VariantComparison> variants)
    {
        // Simplified sample size calculation
        var currentSampleSize = variants.Sum(v => v.Visitors);
        var recommendedMinimum = 1000 * variants.Count; // 1000 per variant minimum
        
        return Math.Max(recommendedMinimum, currentSampleSize * 2);
    }

    public double GetZScoreForConfidenceLevel(double confidenceLevel)
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

    #endregion
}