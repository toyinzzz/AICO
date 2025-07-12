using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Service for core MCP analytics and calculations
/// Delegates specialized functionality to dedicated services
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
    private readonly IMCPValidationService _mcpValidationService;
    private readonly IMCPStatisticalService _mcpStatisticalService;
    private readonly IMCPReportingAnalysisService _mcpReportingAnalysisService;

    public MCPAnalyticsService(
        IRevenueRepository revenueRepository,
        IAbTestRepository abTestRepository,
        IVariantRepository variantRepository,
        IConversionRepository conversionRepository,
        ILogger<MCPAnalyticsService> logger,
        IProfitTrackingService profitTrackingService,
        ICurrencyConversionService currencyConversionService,
        IMCPValidationService mcpValidationService,
        IMCPStatisticalService mcpStatisticalService,
        IMCPReportingAnalysisService mcpReportingAnalysisService)
    {
        _revenueRepository = revenueRepository;
        _abTestRepository = abTestRepository;
        _variantRepository = variantRepository;
        _conversionRepository = conversionRepository;
        _logger = logger;
        _profitTrackingService = profitTrackingService;
        _currencyConversionService = currencyConversionService;
        _mcpValidationService = mcpValidationService;
        _mcpStatisticalService = mcpStatisticalService;
        _mcpReportingAnalysisService = mcpReportingAnalysisService;
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

    public async Task<MCPInputValidationResult> ValidateMCPInputs(decimal minMcp, int minMcpSampleSize, decimal maxMcp, int maxMcpSampleSize)
    {
        return await _mcpValidationService.ValidateBasicInputsAsync(minMcp, minMcpSampleSize, maxMcp, maxMcpSampleSize);
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

    // Delegate statistical methods to specialized service
    public async Task<MCPStatisticalResult> CalculateMCPWithStatisticalSignificanceAsync(
        decimal controlRevenue, decimal controlCost, int controlConversions, int controlVisitors,
        decimal treatmentRevenue, decimal treatmentCost, int treatmentConversions, int treatmentVisitors,
        double confidenceLevel = 0.95)
    {
        return await _mcpStatisticalService.CalculateMCPWithStatisticalSignificanceAsync(
            controlRevenue, controlCost, controlConversions, controlVisitors,
            treatmentRevenue, treatmentCost, treatmentConversions, treatmentVisitors,
            confidenceLevel);
    }

    // Delegate reporting methods to specialized service
    public async Task<MCPAnalysisReport> GenerateMCPAnalysisReportAsync(
        Guid abTestId, DateTime startDate, DateTime endDate)
    {
        return await _mcpReportingAnalysisService.GenerateMCPAnalysisReportAsync(abTestId, startDate, endDate);
    }

    public async Task<List<MCPTrendData>> GetMCPTrendsAsync(
        Guid abTestId, DateTime startDate, DateTime endDate, string granularity = "daily")
    {
        return await _mcpReportingAnalysisService.GetMCPTrendsAsync(abTestId, startDate, endDate, granularity);
    }

    public async Task<MCPInputValidationResult> ValidateMCPInputsAsync(
        decimal revenue, decimal cost, int conversions, int visitors, string currency = "USD")
    {
        return await _mcpValidationService.ValidateBasicInputsAsync(revenue, cost, conversions, visitors);
    }

    // Delegate confidence interval methods to statistical service
    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalAsync(
        decimal revenue, decimal cost, int conversions, int visitors, double confidenceLevel = 0.95)
    {
        return await _mcpStatisticalService.CalculateMCPWithConfidenceIntervalAsync(
            revenue, cost, conversions, visitors, confidenceLevel);
    }

    public MCPConfidenceInterval CalculateMCPWithConfidence(
        decimal controlRevenue, int controlConversions,
        decimal variantRevenue, int variantConversions,
        double confidenceLevel = 0.95)
    {
        return _mcpStatisticalService.CalculateMCPWithConfidence(
            controlRevenue, controlConversions, variantRevenue, variantConversions, confidenceLevel);
    }

    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(
        Guid abTestId, DateTime startDate, DateTime endDate, double confidenceLevel = 0.95)
    {
        return await _mcpStatisticalService.CalculateMCPWithConfidenceIntervalsAsync(
            abTestId, startDate, endDate, confidenceLevel);
    }

    public async Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(
        Guid abTestId, double confidenceLevel = 0.95)
    {
        return await _mcpStatisticalService.CalculateMCPWithConfidenceIntervalsAsync(abTestId, confidenceLevel);
    }

    public async Task<MCPAnalysisResult> GenerateMCPAnalysisAsync(
        Guid abTestId, DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _mcpReportingAnalysisService.GenerateMCPAnalysisAsync(abTestId, startDate, endDate);
    }

    // Legacy methods - kept for backward compatibility but not implemented
    public Task<decimal> CalculateMCP(decimal revenue, decimal cost, int conversions, int visitors)
    {
        throw new NotImplementedException("Use CalculateMCPAsync instead");
    }

    public Task<decimal> CalculateMCPWithCurrency(decimal revenue, decimal cost, int conversions, int visitors, string currency)
    {
        throw new NotImplementedException("Use CalculateMCPWithCurrencyAsync instead");
    }

    public Task<List<MCPVariantResult>> CalculateMCPForMultipleVariants(List<VariantData> variants)
    {
        throw new NotImplementedException("Use CalculateMCPForMultipleVariantsAsync instead");
    }

    public Task<MCPStatisticalResult> CalculateMCPWithSignificance(
        decimal controlRevenue, decimal controlCost, int controlConversions, int controlVisitors,
        decimal treatmentRevenue, decimal treatmentCost, int treatmentConversions, int treatmentVisitors,
        double confidenceLevel = 0.95)
    {
        throw new NotImplementedException("Use CalculateMCPWithStatisticalSignificanceAsync instead");
    }
}