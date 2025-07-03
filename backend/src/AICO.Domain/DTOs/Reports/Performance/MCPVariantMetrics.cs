using System;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Detailed metrics for individual test variants
/// </summary>
public record MCPVariantMetrics(
    string VariantId,
    string VariantName,
    int TotalVisitors,
    int UniqueVisitors,
    int Conversions,
    decimal ConversionRate,
    decimal Revenue,
    decimal AverageOrderValue,
    decimal BounceRate,
    decimal TimeOnPage,
    int PageViews,
    decimal ClickThroughRate
);