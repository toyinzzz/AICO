using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for MCP reporting and trend analysis
/// Handles report generation and trend calculations
/// </summary>
public interface IMCPReportingAnalysisService
{
    /// <summary>
    /// Generate comprehensive MCP analysis report
    /// </summary>
    Task<MCPAnalysisReport> GenerateMCPAnalysisReportAsync(Guid abTestId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Generate MCP analysis (alias for report generation)
    /// </summary>
    Task<MCPAnalysisReport> GenerateMCPAnalysisAsync(Guid abTestId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Get MCP trends over time with specified granularity
    /// </summary>
    Task<List<MCPTrendPoint>> GetMCPTrendsAsync(Guid abTestId, DateTime startDate, DateTime endDate, string granularity = "daily");

    /// <summary>
    /// Generate statistical insights for MCP analysis
    /// </summary>
    MCPStatisticalInsights GenerateStatisticalInsights(List<VariantComparison> variantResults);

    /// <summary>
    /// Create variant comparison results
    /// </summary>
    Task<List<VariantComparison>> CreateVariantComparisonsAsync(Guid abTestId, DateTime startDate, DateTime endDate);
}