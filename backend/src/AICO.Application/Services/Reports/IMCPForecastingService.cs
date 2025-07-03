using AICO.Domain.DTOs;

using AICO.Domain.DTOs.Reports.Analytics;
using AICO.Domain.DTOs.Reports.Financial;

namespace AICO.Application.Services.Reports;

/// <summary>
/// Service responsible for MCP forecasting and predictive analysis
/// Follows Single Responsibility Principle - handles only forecasting
/// </summary>
public interface IMCPForecastingService
{
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
    /// Generate MCP statistical significance report
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="confidenceLevel">Statistical confidence level</param>
    /// <returns>Detailed statistical significance analysis</returns>
    Task<MCPStatisticalReport> GenerateStatisticalReportAsync(Guid abTestId, double confidenceLevel = 0.95);
}