using System;
using System.Collections.Generic;

namespace AICO.Domain.DTOs.Monitoring;

/// <summary>
/// Alert report containing system and performance alerts
/// </summary>
public record MCPAlertReport(
    DateTime ReportDate,
    List<MCPAlert> ActiveAlerts,
    List<MCPAlert> ResolvedAlerts,
    MCPAlertSummary AlertSummary,
    MCPAlertTrends AlertTrends,
    List<MCPAlertRecommendation> Recommendations
);

/// <summary>
/// Individual alert with detailed information
/// </summary>
public record MCPAlert(
    string AlertId,
    string AlertType,
    string Severity,
    string Title,
    string Description,
    DateTime TriggeredAt,
    DateTime? ResolvedAt,
    string Status,
    MCPAlertSource Source,
    MCPAlertMetadata Metadata,
    List<string> AffectedComponents,
    string AssignedTo
);

/// <summary>
/// Source information for alerts
/// </summary>
public record MCPAlertSource(
    string SourceType,
    string SourceId,
    string SourceName,
    Dictionary<string, object> SourceMetadata
);

/// <summary>
/// Additional metadata for alerts
/// </summary>
public record MCPAlertMetadata(
    Dictionary<string, object> CustomFields,
    List<string> Tags,
    string Category,
    decimal Threshold,
    decimal ActualValue
);

/// <summary>
/// Summary of alert statistics
/// </summary>
public record MCPAlertSummary(
    int TotalAlerts,
    int ActiveAlerts,
    int ResolvedAlerts,
    Dictionary<string, int> AlertsBySeverity,
    Dictionary<string, int> AlertsByType,
    decimal AverageResolutionTime,
    string MostCommonAlertType
);

/// <summary>
/// Alert trends and patterns
/// </summary>
public record MCPAlertTrends(
    List<MCPAlertTrendPoint> TrendData,
    string TrendDirection,
    decimal TrendStrength,
    List<string> TrendFactors,
    MCPAlertFrequency Frequency
);

/// <summary>
/// Individual alert trend data point
/// </summary>
public record MCPAlertTrendPoint(
    DateTime Date,
    int AlertCount,
    Dictionary<string, int> AlertsByType,
    decimal AverageResolutionTime
);

/// <summary>
/// Alert frequency analysis
/// </summary>
public record MCPAlertFrequency(
    Dictionary<string, int> HourlyDistribution,
    Dictionary<string, int> DailyDistribution,
    Dictionary<string, int> WeeklyDistribution,
    string PeakAlertTime
);

/// <summary>
/// Recommendations for alert management
/// </summary>
public record MCPAlertRecommendation(
    string RecommendationType,
    string Recommendation,
    string Rationale,
    string Priority,
    List<string> ActionItems,
    decimal ExpectedImpact
);

/// <summary>
/// Real-time metrics for monitoring dashboards
/// </summary>
public record MCPRealTimeMetrics(
    DateTime Timestamp,
    MCPSystemMetrics SystemMetrics,
    MCPPerformanceMetrics PerformanceMetrics,
    MCPTrafficMetrics TrafficMetrics,
    MCPErrorMetrics ErrorMetrics,
    MCPResourceMetrics ResourceMetrics
);

/// <summary>
/// System-level metrics
/// </summary>
public record MCPSystemMetrics(
    decimal CpuUsage,
    decimal MemoryUsage,
    decimal DiskUsage,
    decimal NetworkUsage,
    string SystemHealth,
    int ActiveConnections
);

/// <summary>
/// Performance-related metrics
/// </summary>
public record MCPPerformanceMetrics(
    decimal ResponseTime,
    decimal Throughput,
    decimal Latency,
    decimal SuccessRate,
    int RequestsPerSecond,
    decimal DatabaseResponseTime
);

/// <summary>
/// Traffic and user metrics
/// </summary>
public record MCPTrafficMetrics(
    int CurrentUsers,
    int PageViews,
    int UniqueVisitors,
    decimal BounceRate,
    Dictionary<string, int> TrafficSources,
    Dictionary<string, int> DeviceTypes
);

/// <summary>
/// Error and exception metrics
/// </summary>
public record MCPErrorMetrics(
    int ErrorCount,
    decimal ErrorRate,
    Dictionary<string, int> ErrorsByType,
    List<MCPRecentError> RecentErrors,
    Dictionary<string, int> ErrorsByEndpoint
);

/// <summary>
/// Recent error information
/// </summary>
public record MCPRecentError(
    DateTime Timestamp,
    string ErrorType,
    string ErrorMessage,
    string Source,
    string Severity
);

/// <summary>
/// Resource utilization metrics
/// </summary>
public record MCPResourceMetrics(
    MCPDatabaseMetrics Database,
    MCPCacheMetrics Cache,
    MCPQueueMetrics Queue,
    MCPStorageMetrics Storage
);

/// <summary>
/// Database performance metrics
/// </summary>
public record MCPDatabaseMetrics(
    int ActiveConnections,
    decimal QueryResponseTime,
    int QueriesPerSecond,
    decimal DatabaseSize,
    int SlowQueries
);

/// <summary>
/// Cache performance metrics
/// </summary>
public record MCPCacheMetrics(
    decimal HitRate,
    decimal MissRate,
    int CacheSize,
    decimal CacheUtilization,
    int EvictionsPerSecond
);

/// <summary>
/// Queue processing metrics
/// </summary>
public record MCPQueueMetrics(
    int QueueLength,
    decimal ProcessingRate,
    int FailedJobs,
    decimal AverageProcessingTime,
    int ActiveWorkers
);

/// <summary>
/// Storage utilization metrics
/// </summary>
public record MCPStorageMetrics(
    decimal StorageUsed,
    decimal StorageAvailable,
    decimal StorageUtilization,
    int FilesStored,
    decimal AverageFileSize
);

/// <summary>
/// Health check results for system components
/// </summary>
public record MCPHealthCheck(
    DateTime CheckTime,
    string OverallStatus,
    List<MCPComponentHealth> ComponentHealth,
    MCPHealthSummary Summary,
    List<string> HealthIssues
);

/// <summary>
/// Health status of individual components
/// </summary>
public record MCPComponentHealth(
    string ComponentName,
    string ComponentType,
    string Status,
    decimal ResponseTime,
    string LastChecked,
    Dictionary<string, object> HealthMetrics
);

/// <summary>
/// Summary of overall system health
/// </summary>
public record MCPHealthSummary(
    int TotalComponents,
    int HealthyComponents,
    int UnhealthyComponents,
    int WarningComponents,
    decimal OverallHealthScore,
    string HealthTrend
);

/// <summary>
/// Performance threshold monitoring
/// </summary>
public record MCPThresholdMonitoring(
    List<MCPThreshold> Thresholds,
    List<MCPThresholdViolation> Violations,
    MCPThresholdSummary Summary
);

/// <summary>
/// Individual performance threshold
/// </summary>
public record MCPThreshold(
    string ThresholdId,
    string MetricName,
    decimal WarningThreshold,
    decimal CriticalThreshold,
    string ThresholdType,
    bool IsEnabled
);

/// <summary>
/// Threshold violation information
/// </summary>
public record MCPThresholdViolation(
    string ThresholdId,
    string MetricName,
    decimal ActualValue,
    decimal ThresholdValue,
    string ViolationType,
    DateTime ViolationTime,
    string Status
);

/// <summary>
/// Summary of threshold monitoring
/// </summary>
public record MCPThresholdSummary(
    int TotalThresholds,
    int ActiveViolations,
    int ResolvedViolations,
    Dictionary<string, int> ViolationsByType,
    string MostViolatedThreshold
);