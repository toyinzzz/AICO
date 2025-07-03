using System;
using System.Collections.Generic;
using AICO.Domain.DTOs.Reports.Executive;
using AICO.Domain.DTOs.Reports.Performance;
using AICO.Domain.DTOs.Reports.Analytics;
using AICO.Domain.DTOs.Reports.Segmentation;
using AICO.Domain.DTOs.Reports.Financial;
using AICO.Domain.DTOs.Monitoring;
using AICO.Domain.DTOs.Configuration;
using AICO.Domain.DTOs.Common;

namespace AICO.Domain.DTOs;

/// <summary>
/// This file now serves as a central reference point for all MCP reporting DTOs.
/// The actual DTOs have been organized into domain-specific namespaces:
/// 
/// - Executive Reports: AICO.Domain.DTOs.Reports.Executive
/// - Performance Reports: AICO.Domain.DTOs.Reports.Performance  
/// - Analytics Reports: AICO.Domain.DTOs.Reports.Analytics
/// - Segmentation Reports: AICO.Domain.DTOs.Reports.Segmentation
/// - Financial Reports: AICO.Domain.DTOs.Reports.Financial
/// - Monitoring: AICO.Domain.DTOs.Monitoring
/// - Configuration: AICO.Domain.DTOs.Configuration
/// - Common: AICO.Domain.DTOs.Common
/// 
/// This refactoring improves:
/// - Single Responsibility Principle adherence
/// - Code organization and maintainability
/// - Navigation and discoverability
/// - Reduced merge conflicts
/// - Better separation of concerns
/// - Enhanced testability
/// </summary>
public static class MCPReportingDTOsReference
{
    /// <summary>
    /// Reference to all available report types organized by domain
    /// </summary>
    public static readonly Dictionary<string, Type> ReportTypes = new()
    {
        // Executive Reports
        { "ExecutiveSummary", typeof(MCPExecutiveSummary) },
        
        // Performance Reports
        { "PerformanceReport", typeof(MCPPerformanceReport) },
        { "ComparisonReport", typeof(MCPComparisonReport) },
        
        // Analytics Reports
        
        { "ForecastReport", typeof(MCPForecastReport) },
        { "DashboardData", typeof(MCPDashboardData) },
        
        // Segmentation Reports
        { "SegmentationReport", typeof(MCPSegmentationReport) },
        
        // Financial Reports
        { "ROIReport", typeof(MCPROIReport) },
        { "StatisticalReport", typeof(MCPStatisticalReport) },
        
        // Monitoring
        { "AlertReport", typeof(MCPAlertReport) },
        { "RealTimeMetrics", typeof(AICO.Domain.DTOs.Monitoring.MCPRealTimeMetrics) },
        
        // Configuration
        { "ReportSchedule", typeof(MCPReportSchedule) },
        { "ReportConfiguration", typeof(MCPReportConfiguration) },
        { "ReportTemplate", typeof(MCPReportTemplate) },
        
        // Common
        { "ExportResult", typeof(MCPExportResult) }
    };
    
    /// <summary>
    /// Gets the namespace for a specific report domain
    /// </summary>
    public static string GetNamespaceForDomain(string domain) => domain.ToLowerInvariant() switch
    {
        "executive" => "AICO.Domain.DTOs.Reports.Executive",
        "performance" => "AICO.Domain.DTOs.Reports.Performance",
        "analytics" => "AICO.Domain.DTOs.Reports.Analytics",
        "segmentation" => "AICO.Domain.DTOs.Reports.Segmentation",
        "financial" => "AICO.Domain.DTOs.Reports.Financial",
        "monitoring" => "AICO.Domain.DTOs.Monitoring",
        "configuration" => "AICO.Domain.DTOs.Configuration",
        "common" => "AICO.Domain.DTOs.Common",
        _ => "AICO.Domain.DTOs"
    };
}