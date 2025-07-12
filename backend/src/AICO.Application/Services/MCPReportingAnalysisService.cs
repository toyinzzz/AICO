using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;
using AICO.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Service for MCP reporting and trend analysis
/// Handles report generation, trend analysis, and statistical insights
/// </summary>
public class MCPReportingAnalysisService : IMCPReportingAnalysisService
{
    private readonly IAbTestRepository _abTestRepository;
    private readonly IVariantRepository _variantRepository;
    private readonly IConversionRepository _conversionRepository;
    private readonly IRevenueRepository _revenueRepository;
    private readonly IMCPAnalyticsService _mcpAnalyticsService;
    private readonly IMCPStatisticalService _mcpStatisticalService;
    private readonly ILogger<MCPReportingAnalysisService> _logger;

    public MCPReportingAnalysisService(
        IAbTestRepository abTestRepository,
        IVariantRepository variantRepository,
        IConversionRepository conversionRepository,
        IRevenueRepository revenueRepository,
        IMCPAnalyticsService mcpAnalyticsService,
        IMCPStatisticalService mcpStatisticalService,
        ILogger<MCPReportingAnalysisService> logger)
    {
        _abTestRepository = abTestRepository;
        _variantRepository = variantRepository;
        _conversionRepository = conversionRepository;
        _revenueRepository = revenueRepository;
        _mcpAnalyticsService = mcpAnalyticsService;
        _mcpStatisticalService = mcpStatisticalService;
        _logger = logger;
    }

    public async Task<MCPAnalysisReport> GenerateMCPAnalysisReportAsync(
        Guid abTestId, DateTime startDate, DateTime endDate)
    {
        try
        {
            _logger.LogInformation("Generating MCP analysis report for A/B test {AbTestId} from {StartDate} to {EndDate}", abTestId, startDate, endDate);

            // Get A/B test data
            var abTest = await _abTestRepository.GetByIdAsync(abTestId);
            if (abTest == null)
            {
                throw new ArgumentException($"A/B test with ID {abTestId} not found");
            }

            // Get variants for the A/B test
            var variants = (await _variantRepository.GetByAbTestIdAsync(abTestId)).ToList();
            if (!variants.Any())
            {
                throw new InvalidOperationException("No variants found for the A/B test");
            }

            // Get revenue and conversion data within date range
            var revenues = await _revenueRepository.GetByAbTestIdAsync(abTestId);
            var conversions = await _conversionRepository.GetByAbTestIdAndDateRangeAsync(abTestId, startDate, endDate);

            // Filter revenue data by date range
            revenues = revenues.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).ToList();

            // Calculate overall MCP
            var totalRevenue = revenues.Sum(r => r.Amount);
            var totalCost = variants.Sum(v => v.Cost);
            var totalConversions = conversions.Count();
            var totalVisitors = variants.Sum(v => v.Visitors);

            var overallMCP = await _mcpAnalyticsService.CalculateMCPAsync(totalRevenue, totalCost, totalConversions, totalVisitors);

            // Calculate MCP for each variant
            var variantComparisons = new List<VariantComparison>();
            foreach (var variant in variants)
            {
                var variantRevenue = revenues.Where(r => r.VariantId == variant.Id).Sum(r => r.Amount);
                var variantConversions = conversions.Count(c => c.VariantId == variant.Id);
                var variantMCP = await _mcpAnalyticsService.CalculateMCPAsync(variantRevenue, variant.Cost, variantConversions, variant.Visitors);

                variantComparisons.Add(new VariantComparison
                {
                    VariantId = variant.Id,
                    VariantName = variant.Name,
                    IsControl = variant.IsControl,
                    MCP = variantMCP,
                    Revenue = variantRevenue,
                    Cost = variant.Cost,
                    Conversions = variantConversions,
                    Visitors = variant.Visitors,
                    ConversionRate = variant.Visitors > 0 ? (double)variantConversions / variant.Visitors : 0
                });
            }

            // Generate statistical insights
            var statisticalInsights = await GenerateStatisticalInsights(variantComparisons);

            return new MCPAnalysisReport
            {
                AbTestId = abTestId,
                AbTestName = abTest.Name,
                StartDate = startDate,
                EndDate = endDate,
                OverallMCP = overallMCP,
                VariantComparisons = variantComparisons,
                StatisticalInsights = statisticalInsights,
                GeneratedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating MCP analysis report for A/B test {AbTestId}", abTestId);
            throw;
        }
    }

    public async Task<MCPAnalysisResult> GenerateMCPAnalysisAsync(
        Guid abTestId, DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var effectiveStartDate = startDate ?? DateTime.MinValue;
            var effectiveEndDate = endDate ?? DateTime.MaxValue;

            _logger.LogInformation("Generating MCP analysis for A/B test {AbTestId} from {StartDate} to {EndDate}", abTestId, effectiveStartDate, effectiveEndDate);

            // Get A/B test data
            var abTest = await _abTestRepository.GetByIdAsync(abTestId);
            if (abTest == null)
            {
                throw new ArgumentException($"A/B test with ID {abTestId} not found");
            }

            // Get variants for the A/B test
            var variants = (await _variantRepository.GetByAbTestIdAsync(abTestId)).ToList();
            if (!variants.Any())
            {
                throw new InvalidOperationException("No variants found for the A/B test");
            }

            // Get revenue and conversion data
            var revenues = await _revenueRepository.GetByAbTestIdAsync(abTestId);
            var conversions = await _conversionRepository.GetByAbTestIdAndDateRangeAsync(abTestId, effectiveStartDate, effectiveEndDate);

            // Filter revenue data by date range if dates are specified
            if (startDate.HasValue || endDate.HasValue)
            {
                revenues = revenues.Where(r => r.CreatedAt >= effectiveStartDate && r.CreatedAt <= effectiveEndDate).ToList();
            }

            // Calculate MCP for each variant
            var variantResults = new List<VariantMCPResult>();
            foreach (var variant in variants)
            {
                var variantRevenue = revenues.Where(r => r.VariantId == variant.Id).Sum(r => r.Amount);
                var variantConversions = conversions.Count(c => c.VariantId == variant.Id);
                var variantMCP = await _mcpAnalyticsService.CalculateMCPAsync(variantRevenue, variant.Cost, variantConversions, variant.Visitors);

                variantResults.Add(new VariantMCPResult
                {
                    VariantId = variant.Id,
                    VariantName = variant.Name,
                    IsControl = variant.IsControl,
                    MCP = variantMCP,
                    Revenue = variantRevenue,
                    Cost = variant.Cost,
                    Conversions = variantConversions,
                    Visitors = variant.Visitors
                });
            }

            // Calculate overall metrics
            var totalRevenue = variantResults.Sum(v => v.Revenue);
            var totalCost = variantResults.Sum(v => v.Cost);
            var totalConversions = variantResults.Sum(v => v.Conversions);
            var totalVisitors = variantResults.Sum(v => v.Visitors);
            var overallMCP = await _mcpAnalyticsService.CalculateMCPAsync(totalRevenue, totalCost, totalConversions, totalVisitors);

            return new MCPAnalysisResult
            {
                AbTestId = abTestId,
                OverallMCP = overallMCP,
                VariantResults = variantResults,
                TotalRevenue = totalRevenue,
                TotalCost = totalCost,
                TotalConversions = totalConversions,
                TotalVisitors = totalVisitors,
                AnalysisDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating MCP analysis for A/B test {AbTestId}", abTestId);
            throw;
        }
    }

    public async Task<List<MCPTrendData>> GetMCPTrendsAsync(
        Guid abTestId, DateTime startDate, DateTime endDate, string granularity = "daily")
    {
        try
        {
            _logger.LogInformation("Getting MCP trends for A/B test {AbTestId} from {StartDate} to {EndDate} with {Granularity} granularity", abTestId, startDate, endDate, granularity);

            var trends = new List<MCPTrendData>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var periodEnd = granularity.ToLower() switch
                {
                    "hourly" => currentDate.AddHours(1),
                    "daily" => currentDate.AddDays(1),
                    "weekly" => currentDate.AddDays(7),
                    "monthly" => currentDate.AddMonths(1),
                    _ => currentDate.AddDays(1)
                };

                // Get data for this period
                var revenues = await _revenueRepository.GetByAbTestIdAsync(abTestId);
                var conversions = await _conversionRepository.GetByAbTestIdAndDateRangeAsync(abTestId, currentDate, periodEnd);
                var variants = await _variantRepository.GetByAbTestIdAsync(abTestId);

                // Filter revenue data by period
                revenues = revenues.Where(r => r.CreatedAt >= currentDate && r.CreatedAt < periodEnd).ToList();

                if (revenues.Any() || conversions.Any())
                {
                    var totalRevenue = revenues.Sum(r => r.Amount);
                    var totalCost = variants.Sum(v => v.Cost);
                    var totalConversions = conversions.Count();
                    var totalVisitors = variants.Sum(v => v.Visitors);

                    var periodMCP = await _mcpAnalyticsService.CalculateMCPAsync(totalRevenue, totalCost, totalConversions, totalVisitors);

                    trends.Add(new MCPTrendData
                    {
                        Date = currentDate,
                        MCP = periodMCP,
                        Revenue = totalRevenue,
                        Cost = totalCost,
                        Conversions = totalConversions,
                        Visitors = totalVisitors
                    });
                }

                currentDate = periodEnd;
            }

            return trends;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting MCP trends for A/B test {AbTestId}", abTestId);
            throw;
        }
    }

    public async Task<StatisticalInsights> GenerateStatisticalInsights(List<VariantComparison> variants)
    {
        try
        {
            _logger.LogInformation("Generating statistical insights for {VariantCount} variants", variants.Count);

            if (variants.Count < 2)
            {
                return new StatisticalInsights
                {
                    IsStatisticallySignificant = false,
                    ConfidenceLevel = 0,
                    PValue = 1.0,
                    StatisticalPower = 0,
                    RecommendedSampleSize = 1000,
                    Insights = new List<string> { "At least 2 variants are required for statistical analysis." }
                };
            }

            var controlVariant = variants.FirstOrDefault(v => v.IsControl);
            var treatmentVariants = variants.Where(v => !v.IsControl).ToList();

            if (controlVariant == null || !treatmentVariants.Any())
            {
                return new StatisticalInsights
                {
                    IsStatisticallySignificant = false,
                    ConfidenceLevel = 0,
                    PValue = 1.0,
                    StatisticalPower = 0,
                    RecommendedSampleSize = 1000,
                    Insights = new List<string> { "Both control and treatment variants are required for statistical analysis." }
                };
            }

            // Calculate statistical significance for the best performing treatment
            var bestTreatment = treatmentVariants.OrderByDescending(v => v.MCP).First();
            
            var statisticalResult = await _mcpStatisticalService.CalculateMCPWithStatisticalSignificanceAsync(
                controlVariant.Revenue, controlVariant.Cost, controlVariant.Conversions, controlVariant.Visitors,
                bestTreatment.Revenue, bestTreatment.Cost, bestTreatment.Conversions, bestTreatment.Visitors,
                0.95);

            var statisticalPower = _mcpStatisticalService.CalculateStatisticalPower(variants);
            var recommendedSampleSize = _mcpStatisticalService.CalculateRecommendedSampleSize(variants);

            var insights = new List<string>();

            // Generate insights based on results
            if (statisticalResult.IsStatisticallySignificant)
            {
                insights.Add($"The best treatment variant shows a statistically significant improvement of {statisticalResult.MCPImprovement:F2}% over the control.");
            }
            else
            {
                insights.Add("No statistically significant difference found between variants.");
            }

            if (statisticalPower < 0.8)
            {
                insights.Add($"Statistical power is low ({statisticalPower:P1}). Consider increasing sample size to {recommendedSampleSize}.");
            }

            if (statisticalResult.PValue > 0.05)
            {
                insights.Add($"P-value ({statisticalResult.PValue:F4}) suggests results may be due to chance.");
            }

            return new StatisticalInsights
            {
                IsStatisticallySignificant = statisticalResult.IsStatisticallySignificant,
                ConfidenceLevel = statisticalResult.ConfidenceLevel,
                PValue = statisticalResult.PValue,
                StatisticalPower = statisticalPower,
                RecommendedSampleSize = recommendedSampleSize,
                Insights = insights
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating statistical insights");
            throw;
        }
    }

    public async Task<List<VariantComparison>> CreateVariantComparisonsAsync(
        Guid abTestId, DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var effectiveStartDate = startDate ?? DateTime.MinValue;
            var effectiveEndDate = endDate ?? DateTime.MaxValue;

            _logger.LogInformation("Creating variant comparisons for A/B test {AbTestId} from {StartDate} to {EndDate}", abTestId, effectiveStartDate, effectiveEndDate);

            // Get variants for the A/B test
            var variants = (await _variantRepository.GetByAbTestIdAsync(abTestId)).ToList();
            if (!variants.Any())
            {
                throw new InvalidOperationException("No variants found for the A/B test");
            }

            // Get revenue and conversion data
            var revenues = await _revenueRepository.GetByAbTestIdAsync(abTestId);
            var conversions = await _conversionRepository.GetByAbTestIdAndDateRangeAsync(abTestId, effectiveStartDate, effectiveEndDate);

            // Filter revenue data by date range if dates are specified
            if (startDate.HasValue || endDate.HasValue)
            {
                revenues = revenues.Where(r => r.CreatedAt >= effectiveStartDate && r.CreatedAt <= effectiveEndDate).ToList();
            }

            var variantComparisons = new List<VariantComparison>();
            foreach (var variant in variants)
            {
                var variantRevenue = revenues.Where(r => r.VariantId == variant.Id).Sum(r => r.Amount);
                var variantConversions = conversions.Count(c => c.VariantId == variant.Id);
                var variantMCP = await _mcpAnalyticsService.CalculateMCPAsync(variantRevenue, variant.Cost, variantConversions, variant.Visitors);

                variantComparisons.Add(new VariantComparison
                {
                    VariantId = variant.Id,
                    VariantName = variant.Name,
                    IsControl = variant.IsControl,
                    MCP = variantMCP,
                    Revenue = variantRevenue,
                    Cost = variant.Cost,
                    Conversions = variantConversions,
                    Visitors = variant.Visitors,
                    ConversionRate = variant.Visitors > 0 ? (double)variantConversions / variant.Visitors : 0
                });
            }

            return variantComparisons;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating variant comparisons for A/B test {AbTestId}", abTestId);
            throw;
        }
    }
}