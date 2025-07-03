using AICO.Domain.DTOs;

using AICO.Domain.DTOs.Configuration;
using AICO.Domain.DTOs.Common;

namespace AICO.Application.Services.Reports;

/// <summary>
/// Service responsible for MCP report export and scheduling
/// Follows Single Responsibility Principle - handles only export functionality
/// </summary>
public interface IMCPExportService
{
    /// <summary>
    /// Export MCP report to various formats
    /// </summary>
    /// <param name="reportData">Report data to export</param>
    /// <param name="format">Export format (PDF, Excel, CSV, JSON)</param>
    /// <returns>Exported report as byte array</returns>
    Task<MCPExportResult> ExportReportAsync(object reportData, string format);

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