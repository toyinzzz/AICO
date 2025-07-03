using System;

namespace AICO.Domain.DTOs.Monitoring
{
    public record MCPAlertThresholds
    (
        decimal MinMCPThreshold,
        decimal MaxMCPThreshold,
        double MinConversionRate,
        double MaxConversionRate,
        int MinSampleSize,
        double SignificanceThreshold,
        decimal RevenueDropThreshold
    );
}