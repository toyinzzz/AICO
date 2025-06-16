namespace AICO.Application.Interfaces.ExternalServices
{
    public interface IAnalyticsService
    {
        Task TrackEventAsync(AnalyticsEvent analyticsEvent);
        Task<AnalyticsReport> GenerateReportAsync(AnalyticsReportRequest request);
        Task<ConversionFunnelData> GetConversionFunnelAsync(Guid campaignId, DateTime startDate, DateTime endDate);
        Task<Dictionary<string, object>> GetRealTimeMetricsAsync(Guid campaignId);
    }

    public record AnalyticsEvent(
        string EventType,
        Guid UserId,
        Guid? CampaignId,
        Guid? VariantId,
        Dictionary<string, object> Properties,
        DateTime Timestamp
    );

    public record AnalyticsReportRequest(
        Guid CampaignId,
        DateTime StartDate,
        DateTime EndDate,
        string[] Metrics,
        string GroupBy
    );

    public record AnalyticsReport(
        string ReportId,
        DateTime GeneratedAt,
        Dictionary<string, object> Summary,
        AnalyticsDataPoint[] DataPoints
    );

    public record AnalyticsDataPoint(
        DateTime Timestamp,
        Dictionary<string, object> Metrics
    );

    public record ConversionFunnelData(
        ConversionStep[] Steps,
        double OverallConversionRate
    );

    public record ConversionStep(
        string StepName,
        int Users,
        double ConversionRate,
        double DropOffRate
    );
}