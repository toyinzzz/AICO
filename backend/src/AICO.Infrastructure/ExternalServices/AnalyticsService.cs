using AICO.Application.Interfaces.ExternalServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Infrastructure.ExternalServices
{
    public class AnalyticsService : IAnalyticsService
    {
        public Task TrackEventAsync(AnalyticsEvent analyticsEvent)
        {
            throw new NotImplementedException();
        }

        public Task<AnalyticsReport> GenerateReportAsync(AnalyticsReportRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ConversionFunnelData> GetConversionFunnelAsync(Guid campaignId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetRealTimeMetricsAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }
    }
}