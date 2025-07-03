using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;
using AICO.Domain.Entities;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for MCP (Maximum Customer Profit) analytics and calculations
/// Core MVP service for profit optimization analysis
/// </summary>
public interface IMCPAnalyticsService
{
    /// <summary>
    /// Calculate MCP between control and variant with basic validation
    /// </summary>
    /// <param name="controlRevenue">Total revenue from control group</param>
    /// <param name="controlConversions">Number of conversions in control group</param>
    /// <param name="variantRevenue">Total revenue from variant group</param>
    /// <param name="variantConversions">Number of conversions in variant group</param>
    /// <returns>MCP percentage</returns>
    decimal CalculateMCP(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions);

    /// <summary>
    /// Calculate MCP with currency normalization support
    /// </summary>
    /// <param name="controlRevenue">Control group revenue</param>
    /// <param name="controlConversions">Control group conversions</param>
    /// <param name="variantRevenue">Variant group revenue</param>
    /// <param name="variantConversions">Variant group conversions</param>
    /// <param name="currency">Currency code for normalization</param>
    /// <returns>Normalized MCP percentage</returns>
    decimal CalculateMCPWithCurrency(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, string currency);

    /// <summary>
    /// Calculate MCP for multiple variants against control
    /// </summary>
    /// <param name="variants">List of variant data including control</param>
    /// <returns>MCP results for each non-control variant</returns>
    List<MCPResult> CalculateMCPForMultipleVariants(List<VariantData> variants);

    /// <summary>
    /// Calculate MCP with statistical significance validation
    /// </summary>
    /// <param name="controlRevenue">Control group revenue</param>
    /// <param name="controlConversions">Control group conversions</param>
    /// <param name="variantRevenue">Variant group revenue</param>
    /// <param name="variantConversions">Variant group conversions</param>
    /// <returns>MCP percentage if statistically significant, null otherwise</returns>
    decimal? CalculateMCPWithSignificance(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions);

    /// <summary>
    /// Generate comprehensive MCP analysis report
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <returns>Detailed MCP analysis report</returns>
    Task<MCPAnalysisReport> GenerateMCPAnalysisAsync(Guid abTestId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Calculate time-based MCP trends
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <param name="interval">Time interval for trend analysis (daily, weekly, monthly)</param>
    /// <returns>MCP trend data points</returns>
    Task<List<MCPTrendPoint>> GetMCPTrendsAsync(Guid abTestId, DateTime startDate, DateTime endDate, string interval = "daily");

    /// <summary>
    /// Validate MCP calculation inputs
    /// </summary>
    /// <param name="controlRevenue">Control revenue</param>
    /// <param name="controlConversions">Control conversions</param>
    /// <param name="variantRevenue">Variant revenue</param>
    /// <param name="variantConversions">Variant conversions</param>
    /// <returns>Validation result with error messages if invalid</returns>
    Task<MCPInputValidationResult> ValidateMCPInputs(decimal minMcp, int minMcpSampleSize, decimal maxMcp, int maxMcpSampleSize);

    /// <summary>
    /// Calculate confidence intervals for MCP
    /// </summary>
    /// <param name="controlRevenue">Control group revenue</param>
    /// <param name="controlConversions">Control group conversions</param>
    /// <param name="variantRevenue">Variant group revenue</param>
    /// <param name="variantConversions">Variant group conversions</param>
    /// <param name="confidenceLevel">Confidence level (e.g., 0.95 for 95%)</param>
    /// <returns>MCP with confidence intervals</returns>
    MCPConfidenceInterval CalculateMCPWithConfidence(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, double confidenceLevel = 0.95);

    /// <summary>
    /// Calculate MCP with confidence intervals for an A/B test
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <param name="confidenceLevel">Confidence level (e.g., 0.95 for 95%)</param>
    /// <returns>MCP with confidence intervals</returns>
    Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(Guid abTestId, DateTime startDate, DateTime endDate, double confidenceLevel = 0.95);
}