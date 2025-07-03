using AICO.Domain.DTOs;

using AICO.Domain.DTOs.Reports.Executive;
using AICO.Domain.DTOs.Monitoring;
using AICO.Domain.DTOs.Reports.Analytics;

namespace AICO.Application.Services.Reports;

/// <summary>
/// Service responsible for real-time MCP dashboard and monitoring
/// Follows Single Responsibility Principle - handles only dashboard functionality
/// </summary>
public interface IMCPDashboardService
{
    /// <summary>
    /// Generate MCP dashboard data for real-time monitoring
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="refreshIntervalMinutes">Data refresh interval</param>
    /// <returns>Real-time MCP dashboard data</returns>
    Task<MCPDashboardData> GenerateDashboardDataAsync(Guid abTestId, int refreshIntervalMinutes = 5);

    /// <summary>
    /// Generate MCP alert report based on predefined thresholds
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="alertThresholds">Alert configuration thresholds</param>
    /// <returns>MCP alert report with triggered alerts</returns>
    Task<MCPAlertReport> GenerateAlertReportAsync(Guid abTestId, MCPAlertThresholds alertThresholds);
}