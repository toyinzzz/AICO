using System;
using System.Collections.Generic;

namespace AICO.Domain.DTOs.Common;

/// <summary>
/// Export result information for report exports
/// </summary>
public record MCPExportResult(
    string ExportId,
    string ExportType,
    string FileName,
    string FilePath,
    long FileSize,
    string Status,
    DateTime ExportedAt,
    string ExportedBy,
    MCPExportMetadata Metadata,
    string DownloadUrl,
    DateTime? ExpiresAt
);

/// <summary>
/// Metadata for export operations
/// </summary>
public record MCPExportMetadata(
    string ReportType,
    Dictionary<string, object> ExportParameters,
    int RecordCount,
    string Compression,
    List<string> IncludedSections,
    string Quality
);

/// <summary>
/// Common benchmark metric for comparisons
/// </summary>
public record MCPBenchmarkMetric(
    string MetricName,
    decimal CurrentValue,
    decimal BenchmarkValue,
    decimal Variance,
    string VarianceType,
    string BenchmarkSource,
    DateTime BenchmarkDate
);

/// <summary>
/// Cross-test insights for portfolio analysis
/// </summary>
public record MCPCrossTestInsight(
    string InsightId,
    string InsightType,
    string Title,
    string Description,
    List<string> AffectedTests,
    decimal Confidence,
    MCPInsightImpact Impact,
    List<string> Recommendations,
    DateTime IdentifiedAt
);

/// <summary>
/// Impact assessment for insights
/// </summary>
public record MCPInsightImpact(
    string ImpactLevel,
    decimal PotentialRevenue,
    decimal PotentialConversions,
    string TimeToImpact,
    decimal ImplementationEffort,
    List<string> RiskFactors
);

/// <summary>
/// Portfolio-level metrics across multiple tests
/// </summary>
public class MCPPortfolioMetrics
{
    public int TotalActiveTests { get; set; }
    public int TotalCompletedTests { get; set; }
    public decimal OverallConversionRate { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageTestDuration { get; set; }
    public decimal OverallROI { get; set; }
    public decimal TotalLift { get; set; }
    public decimal WinRate { get; set; }
    public decimal AverageConfidence { get; set; }
    public string PerformanceTrend { get; set; } = string.Empty;
    
    // Keep existing properties for backward compatibility
    public MCPPortfolioPerformance Performance { get; set; } = new(0m, 0m, 0m, 0m, string.Empty, new Dictionary<string, decimal>());
    public List<MCPTestSummary> TopPerformingTests { get; set; } = new();
    public List<MCPTestSummary> UnderperformingTests { get; set; } = new();
}

/// <summary>
/// Portfolio performance summary
/// </summary>
public record MCPPortfolioPerformance(
    decimal OverallROI,
    decimal TotalLift,
    decimal WinRate,
    decimal AverageConfidence,
    string PerformanceTrend,
    Dictionary<string, decimal> MetricsByCategory
);

/// <summary>
/// Summary information for individual tests
/// </summary>
public record MCPTestSummary(
    string TestId,
    string TestName,
    string Status,
    decimal ConversionRate,
    decimal Revenue,
    decimal Confidence,
    DateTime StartDate,
    DateTime? EndDate,
    string Category
);

/// <summary>
/// Benchmark analysis comparing against industry standards
/// </summary>
public record MCPBenchmarkAnalysis(
    string BenchmarkType,
    string Industry,
    List<MCPBenchmarkMetric> Metrics,
    MCPBenchmarkSummary Summary,
    List<string> Recommendations,
    DateTime BenchmarkDate
);

/// <summary>
/// Summary of benchmark comparison
/// </summary>
public record MCPBenchmarkSummary(
    string OverallPerformance,
    int MetricsAboveBenchmark,
    int MetricsBelowBenchmark,
    decimal AverageVariance,
    List<string> StrengthAreas,
    List<string> ImprovementAreas
);

/// <summary>
/// Common pagination information
/// </summary>
public record MCPPagination(
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
);

/// <summary>
/// Common sorting configuration
/// </summary>
public record MCPSorting(
    string SortBy,
    string SortDirection,
    List<MCPSortField> SecondarySort
);

/// <summary>
/// Individual sort field
/// </summary>
public record MCPSortField(
    string FieldName,
    string Direction,
    int Priority
);

/// <summary>
/// Common error information
/// </summary>
public record MCPError(
    string ErrorCode,
    string ErrorMessage,
    string ErrorType,
    DateTime Timestamp,
    Dictionary<string, object> ErrorDetails,
    string StackTrace
);

/// <summary>
/// API response wrapper
/// </summary>
public record MCPApiResponse<T>(
    bool Success,
    T Data,
    MCPError Error,
    MCPPagination Pagination,
    Dictionary<string, object> Metadata
);

/// <summary>
/// Common date range specification
/// </summary>
public record MCPDateRange(
    DateTime StartDate,
    DateTime EndDate,
    string RangeType,
    string TimeZone
);

/// <summary>
/// Common metric definition
/// </summary>
public record MCPMetricDefinition(
    string MetricName,
    string DisplayName,
    string Description,
    string DataType,
    string Unit,
    string Category,
    bool IsCalculated,
    string CalculationFormula
);

/// <summary>
/// Common filter specification
/// </summary>
public record MCPFilter(
    string FieldName,
    string Operator,
    object Value,
    string DataType,
    bool IsRequired
);

/// <summary>
/// Common aggregation specification
/// </summary>
public record MCPAggregation(
    string AggregationType,
    string FieldName,
    string GroupBy,
    Dictionary<string, object> AggregationConfig
);

/// <summary>
/// Common time series data point
/// </summary>
public record MCPTimeSeriesPoint(
    DateTime Timestamp,
    decimal Value,
    string MetricName,
    Dictionary<string, object> Dimensions
);

/// <summary>
/// Common comparison result
/// </summary>
public record MCPComparison(
    string ComparisonType,
    object ValueA,
    object ValueB,
    decimal Difference,
    decimal PercentageChange,
    string ComparisonResult
);

/// <summary>
/// Common audit trail entry
/// </summary>
public record MCPAuditEntry(
    string EntryId,
    string Action,
    string EntityType,
    string EntityId,
    string UserId,
    DateTime Timestamp,
    Dictionary<string, object> Changes,
    string IpAddress
);

/// <summary>
/// Common notification specification
/// </summary>
public record MCPNotification(
    string NotificationId,
    string NotificationType,
    string Title,
    string Message,
    string Severity,
    DateTime CreatedAt,
    bool IsRead,
    Dictionary<string, object> NotificationData
);

/// <summary>
/// Common cache configuration
/// </summary>
public record MCPCacheConfig(
    string CacheKey,
    TimeSpan ExpirationTime,
    string CacheType,
    Dictionary<string, object> CacheParameters,
    bool SlidingExpiration
);

/// <summary>
/// Common validation result
/// </summary>
public record MCPValidationResult(
    bool IsValid,
    List<string> Errors,
    List<string> Warnings,
    Dictionary<string, object> ValidationData
);

/// <summary>
/// Common operation result
/// </summary>
public record MCPOperationResult<T>(
    bool Success,
    T Result,
    string Message,
    List<string> Errors,
    TimeSpan ExecutionTime
);

/// <summary>
/// Common search criteria
/// </summary>
public record MCPSearchCriteria(
    string SearchTerm,
    List<string> SearchFields,
    List<MCPFilter> Filters,
    MCPSorting Sorting,
    MCPPagination Pagination
);

/// <summary>
/// Common configuration setting
/// </summary>
public record MCPConfigSetting(
    string SettingKey,
    object SettingValue,
    string SettingType,
    string Category,
    string Description,
    bool IsRequired,
    object DefaultValue
);

/// <summary>
/// Overall test metrics shared across reports
/// </summary>
public class MCPOverallMetrics
{
    public decimal TotalMCP { get; set; }
    public decimal MCPImprovement { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalConversions { get; set; }
    public decimal AverageOrderValue { get; set; }
    public double ConversionRate { get; set; }
    
    // Keep existing properties for backward compatibility
    public int TotalVisitors { get; set; }
    public decimal RevenuePerVisitor { get; set; }
    public decimal StatisticalSignificance { get; set; }
    public int DaysRunning { get; set; }
    public string TestStatus { get; set; } = string.Empty;
}

/// <summary>
/// Performance metrics for individual variants shared across reports
/// </summary>
public class MCPVariantPerformance
{
    public string VariantId { get; set; } = string.Empty;
    public string VariantName { get; set; } = string.Empty;
    public decimal MCP { get; set; }
    public bool IsControl { get; set; }
    
    // Keep existing properties for backward compatibility
    public int Visitors { get; set; }
    public int Conversions { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal Revenue { get; set; }
    public decimal RevenuePerVisitor { get; set; }
    public decimal ConfidenceInterval { get; set; }
    public string PerformanceRating { get; set; } = string.Empty;
}

/// <summary>
/// Statistical analysis summary shared across reports
/// </summary>
public class MCPStatisticalAnalysis
{
    public bool IsSignificant { get; set; }
    public double PValue { get; set; }
    public double ConfidenceLevel { get; set; }
    public double EffectSize { get; set; }
    public int SampleSize { get; set; }
    public double StatisticalPower { get; set; }
    
    // Keep existing properties for backward compatibility
    public bool IsStatisticallySignificant { get; set; }
    public decimal PowerAnalysis { get; set; }
    public string TestType { get; set; } = string.Empty;
}

/// <summary>
/// Revenue breakdown shared across reports
/// </summary>
public record MCPRevenueBreakdown(
    decimal TotalRevenue,
    decimal RevenueGrowth,
    decimal AverageOrderValue,
    decimal RevenuePerConversion,
    Dictionary<string, decimal> RevenueBySegment,
    Dictionary<string, decimal> RevenueByTimeframe
);