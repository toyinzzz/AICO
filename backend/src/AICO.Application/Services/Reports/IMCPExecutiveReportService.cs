using AICO.Domain.DTOs;

using AICO.Domain.DTOs.Reports.Executive;
using AICO.Domain.DTOs.Reports.Financial;

namespace AICO.Application.Services.Reports;

/// <summary>
/// Service responsible for generating executive-level MCP reports
/// Follows Single Responsibility Principle - handles only executive reporting
/// </summary>
public interface IMCPExecutiveReportService
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
    /// Generate MCP ROI analysis report for executives
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <param name="investmentCost">Total investment cost for the test</param>
    /// <returns>ROI analysis based on MCP improvements</returns>
    Task<MCPROIReport> GenerateROIReportAsync(Guid abTestId, DateTime startDate, DateTime endDate, decimal investmentCost);
}