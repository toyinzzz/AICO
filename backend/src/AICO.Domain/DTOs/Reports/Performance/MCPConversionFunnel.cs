using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Conversion funnel analysis for performance tracking
/// </summary>
public record MCPConversionFunnel(
    List<MCPFunnelStep> Steps,
    decimal OverallConversionRate,
    List<MCPDropOffPoint> DropOffPoints,
    Dictionary<string, decimal> ConversionRatesByStep
);

/// <summary>
/// Individual step in the conversion funnel
/// </summary>
public record MCPFunnelStep(
    string StepName,
    int Visitors,
    decimal ConversionRate,
    decimal DropOffRate,
    string StepType
);

/// <summary>
/// Points where users drop off in the conversion process
/// </summary>
public record MCPDropOffPoint(
    string StepName,
    decimal DropOffRate,
    int UsersLost,
    List<string> PossibleReasons
);