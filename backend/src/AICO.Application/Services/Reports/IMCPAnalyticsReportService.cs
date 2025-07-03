using AICO.Domain.DTOs;

using AnalyticsMCPTrendAnalysis = AICO.Domain.DTOs.Reports.Analytics.MCPTrendAnalysis;
using PerformanceMCPTrendAnalysis = AICO.Domain.DTOs.Reports.Performance.MCPTrendAnalysis;
using AICO.Domain.DTOs.Reports.Analytics;
using AICO.Domain.DTOs.Reports.Executive;
using AICO.Domain.DTOs.Reports.Performance;
using AICO.Domain.DTOs.Reports.Segmentation;

namespace AICO.Application.Services.Reports;

/// <summary>
/// Service responsible for generating analytical MCP reports
/// Follows Single Responsibility Principle - handles only analytical reporting
/// </summary>
public interface IMCPAnalyticsReportService
{
    /// <summary>
    /// Generate detailed MCP performance report
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Report start date</param>
    /// <param name="endDate">Report end date</param>
    /// <param name="includeStatisticalDetails">Include detailed statistical details</param>
    /// <returns>Comprehensive MCP performance report</returns>
    Task<MCPPerformanceReport> GeneratePerformanceReportAsync(Guid abTestId, DateTime startDate, DateTime endDate, bool includeStatisticalDetails = true);

    /// <summary>
    /// Generate MCP comparison report across multiple A/B tests
    /// </summary>
    /// <param name="abTestIds">List of A/B test identifiers to compare</param>
    /// <param name="startDate">Comparison start date</param>
    /// <param name="endDate">Comparison end date</param>
    /// <returns>Cross-test MCP comparison report</returns>
    Task<MCPComparisonReport> GenerateComparisonReportAsync(List<Guid> abTestIds, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Generate MCP trend analysis over time
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <param name="granularity">Time granularity (hourly, daily, weekly, monthly)</param>
    /// <returns>Time-series MCP trend analysis</returns>
    Task<PerformanceMCPTrendAnalysis> GenerateTrendAnalysisAsync(Guid abTestId, DateTime startDate, DateTime endDate, string granularity = "daily");

    /// <summary>
    /// Generate MCP segmentation report by user demographics or behavior
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="segmentationType">Type of segmentation (geographic, demographic, behavioral)</param>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <returns>Segmented MCP analysis report</returns>
    Task<MCPSegmentationReport> GenerateSegmentationReportAsync(Guid abTestId, string segmentationType, DateTime startDate, DateTime endDate);
}