using AICO.Domain.DTOs;
using AICO.Shared.Interfaces;
using System;
using System.Linq;

namespace AICO.Application.Mappers
{
    public class RevenueReportMapper : IMapper<RevenueReportSource, RevenueReport>
    {
        public RevenueReport Map(RevenueReportSource source)
        {
            if (source == null)
            {
                return null;
            }

            Guid? winningVariantId = null;
            if (!string.IsNullOrEmpty(source.WinningVariant) && source.WinningVariant != "N/A" && source.WinningVariant != "Control")
            {
                if (Guid.TryParse(source.WinningVariant, out var parsedGuid))
                {
                    winningVariantId = parsedGuid;
                }
            }

            var revenueByVariant = source.Metrics?.RevenueByVariant ?? new Dictionary<Guid, decimal>();

            return new RevenueReport
            {
                AbTestId = source.CampaignId,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                TotalRevenue = source.Metrics?.TotalRevenue ?? 0,
                TotalConversions = source.Metrics?.Conversions ?? 0,
                AverageOrderValue = source.Metrics?.AverageOrderValue ?? 0,
                RevenueByVariant = revenueByVariant,
                OverallMCP = source.OverallMcp,
                WinningVariantId = winningVariantId
            };
        }

        public RevenueReportSource Map(RevenueReport destination)
        {
            throw new NotImplementedException();
        }
    }
}