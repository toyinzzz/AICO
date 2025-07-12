using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for MCP statistical calculations
/// Handles statistical significance, confidence intervals, and power analysis
/// </summary>
public interface IMCPStatisticalService
{
    /// <summary>
    /// Calculate MCP with statistical significance analysis
    /// </summary>
    Task<MCPStatisticalResult> CalculateMCPWithStatisticalSignificanceAsync(
        decimal controlRevenue, decimal controlCost, int controlConversions, int controlVisitors,
        decimal treatmentRevenue, decimal treatmentCost, int treatmentConversions, int treatmentVisitors,
        double confidenceLevel = 0.95);

    /// <summary>
    /// Calculate MCP with confidence interval
    /// </summary>
    Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalAsync(
        decimal revenue, decimal cost, int conversions, int visitors, double confidenceLevel = 0.95);

    /// <summary>
    /// Calculate MCP with confidence for control vs variant comparison
    /// </summary>
    MCPConfidenceInterval CalculateMCPWithConfidence(
        decimal controlRevenue, int controlConversions, 
        decimal variantRevenue, int variantConversions, 
        double confidenceLevel = 0.95);

    /// <summary>
    /// Calculate MCP with confidence intervals for A/B test
    /// </summary>
    Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(
        Guid abTestId, DateTime startDate, DateTime endDate, double confidenceLevel = 0.95);

    /// <summary>
    /// Calculate MCP with confidence intervals for A/B test (all data)
    /// </summary>
    Task<MCPConfidenceInterval> CalculateMCPWithConfidenceIntervalsAsync(
        Guid abTestId, double confidenceLevel = 0.95);

    /// <summary>
    /// Calculate statistical power for given variants
    /// </summary>
    double CalculateStatisticalPower(List<VariantComparison> variants);

    /// <summary>
    /// Calculate recommended sample size
    /// </summary>
    int CalculateRecommendedSampleSize(List<VariantComparison> variants);

    /// <summary>
    /// Get Z-score for confidence level
    /// </summary>
    double GetZScoreForConfidenceLevel(double confidenceLevel);
}