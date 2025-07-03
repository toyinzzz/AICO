using AICO.Application.Interfaces.Services;
using AICO.Application.Services.Reports;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.Reports.Executive;
using AICO.Domain.DTOs.Reports.Performance;
using AICO.Domain.DTOs.Reports.Analytics;
using AICO.Domain.DTOs.Reports.Segmentation;
using AICO.Domain.DTOs.Reports.Financial;
using AICO.Domain.DTOs.Monitoring;
using AICO.Domain.DTOs.Configuration;
using AICO.Domain.DTOs.Common;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Facade service for MCP reporting functionality
/// Coordinates between specialized reporting services following SOLID principles
/// Acts as a single entry point while delegating to focused services
/// </summary>
public class MCPReportingService : IMCPReportingService
{
    private readonly IMCPExecutiveReportService _executiveReportService;
    private readonly IMCPAnalyticsReportService _analyticsReportService;
    private readonly IMCPForecastingService _forecastingService;
    private readonly IMCPDashboardService _dashboardService;
    private readonly IMCPExportService _exportService;
    private readonly ILogger<MCPReportingService> _logger;

    public MCPReportingService(
        IMCPExecutiveReportService executiveReportService,
        IMCPAnalyticsReportService analyticsReportService,
        IMCPForecastingService forecastingService,
        IMCPDashboardService dashboardService,
        IMCPExportService exportService,
        ILogger<MCPReportingService> logger)
    {
        _executiveReportService = executiveReportService ?? throw new ArgumentNullException(nameof(executiveReportService));
        _analyticsReportService = analyticsReportService ?? throw new ArgumentNullException(nameof(analyticsReportService));
        _forecastingService = forecastingService ?? throw new ArgumentNullException(nameof(forecastingService));
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Executive Reports
    
    public async Task<MCPExecutiveSummary> GenerateExecutiveSummaryAsync(Guid abTestId, DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Delegating executive summary generation to specialized service for test {AbTestId}", abTestId);
        return await _executiveReportService.GenerateExecutiveSummaryAsync(abTestId, startDate, endDate);
    }

    public async Task<MCPROIReport> GenerateROIReportAsync(Guid abTestId, DateTime startDate, DateTime endDate, decimal investmentCost)
    {
        _logger.LogInformation("Delegating ROI report generation to specialized service for test {AbTestId}", abTestId);
        return await _executiveReportService.GenerateROIReportAsync(abTestId, startDate, endDate, investmentCost);
    }

    #endregion

    #region Analytics Reports

    public async Task<MCPPerformanceReport> GeneratePerformanceReportAsync(Guid abTestId, DateTime startDate, DateTime endDate, bool includeStatisticalDetails = true)
    {
        _logger.LogInformation("Delegating performance report generation to specialized service for test {AbTestId}", abTestId);
        return await _analyticsReportService.GeneratePerformanceReportAsync(abTestId, startDate, endDate, includeStatisticalDetails);
    }

    public async Task<MCPComparisonReport> GenerateComparisonReportAsync(List<Guid> abTestIds, DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Delegating comparison report generation to specialized service for {TestCount} tests", abTestIds.Count);
        return await _analyticsReportService.GenerateComparisonReportAsync(abTestIds, startDate, endDate);
    }

    public async Task<AICO.Domain.DTOs.Reports.Performance.MCPTrendAnalysis> GenerateTrendAnalysisAsync(Guid abTestId, DateTime startDate, DateTime endDate, string granularity = "daily")
    {
        _logger.LogInformation("Delegating trend analysis generation to specialized service for test {AbTestId}", abTestId);
        return await _analyticsReportService.GenerateTrendAnalysisAsync(abTestId, startDate, endDate, granularity);
    }

    public async Task<MCPSegmentationReport> GenerateSegmentationReportAsync(Guid abTestId, string segmentationType, DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Delegating segmentation report generation to specialized service for test {AbTestId}", abTestId);
        return await _analyticsReportService.GenerateSegmentationReportAsync(abTestId, segmentationType, startDate, endDate);
    }

    #endregion

    #region Forecasting Reports

    public async Task<MCPForecastReport> GenerateForecastReportAsync(Guid abTestId, DateTime historicalStartDate, DateTime historicalEndDate, int forecastPeriodDays = 30)
    {
        _logger.LogInformation("Delegating forecast report generation to specialized service for test {AbTestId}", abTestId);
        return await _forecastingService.GenerateForecastReportAsync(abTestId, historicalStartDate, historicalEndDate, forecastPeriodDays);
    }

    public async Task<MCPStatisticalReport> GenerateStatisticalReportAsync(Guid abTestId, double confidenceLevel = 0.95)
    {
        _logger.LogInformation("Delegating statistical report generation to specialized service for test {AbTestId}", abTestId);
        return await _forecastingService.GenerateStatisticalReportAsync(abTestId, confidenceLevel);
    }

    #endregion

    #region Dashboard Services

    public async Task<MCPDashboardData> GenerateDashboardDataAsync(Guid abTestId, int refreshIntervalMinutes = 5)
    {
        _logger.LogInformation("Delegating dashboard data generation to specialized service for test {AbTestId}", abTestId);
        return await _dashboardService.GenerateDashboardDataAsync(abTestId, refreshIntervalMinutes);
    }

    public async Task<MCPDashboardData> GetRealTimeDashboardDataAsync(Guid abTestId)
    {
        _logger.LogInformation("Delegating real-time dashboard data generation to specialized service for test {AbTestId}", abTestId);
        return await _dashboardService.GenerateDashboardDataAsync(abTestId, 1); // Use 1 minute refresh for real-time
    }

    public async Task<MCPAlertReport> GenerateAlertReportAsync(Guid abTestId, MCPAlertThresholds alertThresholds)
    {
        _logger.LogInformation("Delegating alert report generation to specialized service for test {AbTestId}", abTestId);
        return await _dashboardService.GenerateAlertReportAsync(abTestId, alertThresholds);
    }

    #endregion

    #region Export Services

    public async Task<MCPExportResult> ExportReportAsync(object reportData, string format)
    {
        _logger.LogInformation("Delegating report export to specialized service for format {Format}", format);
        return await _exportService.ExportReportAsync(reportData, format);
    }

    public async Task<MCPReportSchedule> ScheduleReportAsync(MCPReportConfiguration reportConfig)
    {
        _logger.LogInformation("Delegating report scheduling to specialized service");
        return await _exportService.ScheduleReportAsync(reportConfig);
    }

    public async Task<List<MCPReportTemplate>> GetAvailableTemplatesAsync()
    {
        _logger.LogInformation("Delegating template retrieval to specialized service");
        return await _exportService.GetAvailableTemplatesAsync();
    }

    #endregion
}