using AICO.Domain.DTOs;

using AICO.Domain.DTOs.Reports.Executive;
using AICO.Domain.DTOs.Reports.Performance;
using AICO.Domain.DTOs.Reports.Analytics;
using AICO.Domain.DTOs.Reports.Segmentation;
using AICO.Domain.DTOs.Reports.Financial;
using AICO.Domain.DTOs.Monitoring;
using AICO.Domain.DTOs.Configuration;
using AICO.Domain.DTOs.Common;
using AICO.Domain.DTOs.Reports;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for MCP reporting and dashboard functionality
/// Handles comprehensive MCP analysis, reporting, and data visualization
/// </summary>
public interface IMCPReportingService
{
    /// <summary>
    /// Generate executive summary report for MCP analysis
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Report start date</param>
    /// <param name="endDate">Report end date</param>
    /// <returns>Executive summary with key MCP insights</returns>
    Task<MCPExecutiveSummary> GenerateExecutiveSummaryAsync(Guid abTestId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Generate detailed MCP performance report
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Report start date</param>
    /// <param name="endDate">Report end date</param>
    /// <param name="includeStatisticalDetails">Include detailed statistical analysis</param>
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
    Task<AICO.Domain.DTOs.Reports.Performance.MCPTrendAnalysis> GenerateTrendAnalysisAsync(Guid abTestId, DateTime startDate, DateTime endDate, string granularity = "daily");

    /// <summary>
    /// Generate MCP segmentation report by user demographics or behavior
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="segmentationType">Type of segmentation (geographic, demographic, behavioral)</param>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <returns>Segmented MCP analysis report</returns>
    Task<MCPSegmentationReport> GenerateSegmentationReportAsync(Guid abTestId, string segmentationType, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Generate MCP forecasting report with predictive analysis
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="historicalStartDate">Historical data start date</param>
    /// <param name="historicalEndDate">Historical data end date</param>
    /// <param name="forecastPeriodDays">Number of days to forecast</param>
    /// <returns>MCP forecasting report with predictions</returns>
    Task<MCPForecastReport> GenerateForecastReportAsync(Guid abTestId, DateTime historicalStartDate, DateTime historicalEndDate, int forecastPeriodDays = 30);

    /// <summary>
    /// Generate MCP dashboard data for real-time monitoring
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="refreshIntervalMinutes">Data refresh interval</param>
    /// <returns>Real-time MCP dashboard data</returns>
    Task<MCPDashboardData> GenerateDashboardDataAsync(Guid abTestId, int refreshIntervalMinutes = 5);

    /// <summary>
    /// Get real-time MCP dashboard data
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <returns>Real-time MCP dashboard data</returns>
    Task<MCPDashboardData> GetRealTimeDashboardDataAsync(Guid abTestId);

    /// <summary>
    /// Generate MCP alert report based on predefined thresholds
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="alertThresholds">Alert configuration thresholds</param>
    /// <returns>MCP alert report with triggered alerts</returns>
    Task<MCPAlertReport> GenerateAlertReportAsync(Guid abTestId, MCPAlertThresholds alertThresholds);

    /// <summary>
    /// Export MCP report to various formats
    /// </summary>
    /// <param name="reportData">Report data to export</param>
    /// <param name="format">Export format (PDF, Excel, CSV, JSON)</param>
    /// <returns>Exported report as byte array</returns>
    Task<MCPExportResult> ExportReportAsync(object reportData, string format);

    /// <summary>
    /// Generate MCP ROI analysis report
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <param name="investmentCost">Total investment cost for the test</param>
    /// <returns>ROI analysis based on MCP improvements</returns>
    Task<MCPROIReport> GenerateROIReportAsync(Guid abTestId, DateTime startDate, DateTime endDate, decimal investmentCost);

    /// <summary>
    /// Generate MCP statistical significance report
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="confidenceLevel">Statistical confidence level</param>
    /// <returns>Detailed statistical significance analysis</returns>
    Task<MCPStatisticalReport> GenerateStatisticalReportAsync(Guid abTestId, double confidenceLevel = 0.95);

    /// <summary>
    /// Schedule automated MCP report generation
    /// </summary>
    /// <param name="reportConfig">Report configuration and schedule</param>
    /// <returns>Scheduled report configuration</returns>
    Task<MCPReportSchedule> ScheduleReportAsync(MCPReportConfiguration reportConfig);

    /// <summary>
    /// Get available MCP report templates
    /// </summary>
    /// <returns>List of available report templates</returns>
    Task<List<MCPReportTemplate>> GetAvailableTemplatesAsync();
}