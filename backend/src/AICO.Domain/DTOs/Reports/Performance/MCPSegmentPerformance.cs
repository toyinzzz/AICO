using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Performance metrics by user segment
/// </summary>
public record MCPSegmentPerformance(
    string SegmentName,
    string SegmentCriteria,
    int SegmentSize,
    decimal ConversionRate,
    decimal Revenue,
    decimal EngagementScore,
    Dictionary<string, decimal> CustomMetrics
);