using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Shared.Interfaces;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace AICO.Domain.Services;

public class RevenueTrackingService : IRevenueTrackingService
    {
        private readonly IMapper<Revenue, RevenueDto> _mapper;
        private readonly IMapper<RevenueReportSource, RevenueReport> _reportMapper;
    private readonly IRevenueRepository _revenueRepository;
    private readonly ILogger<RevenueTrackingService> _logger;

    public RevenueTrackingService(IRevenueRepository revenueRepository, ILogger<RevenueTrackingService> logger, IMapper<Revenue, RevenueDto> mapper, IMapper<RevenueReportSource, RevenueReport> reportMapper)
        {
            _revenueRepository = revenueRepository;
            _logger = logger;
            _mapper = mapper;
            _reportMapper = reportMapper;
        }

    public async Task RecordRevenueEventAsync(Guid campaignId, Guid variantId, decimal amount, string currency, string? transactionId = null, bool isControl = false)
    {
        var revenue = new Revenue(
            websiteId: campaignId, // Assuming campaignId maps to websiteId for now
            userId: Guid.Empty, // This needs to be resolved, how to get userId here?
            amount: amount,
            source: "event",
            currency: currency,
            transactionId: transactionId,
            sessionId: null, // This needs to be resolved
            variantId: variantId,
            isControl: isControl
        );

        await _revenueRepository.AddAsync(revenue);
        _logger.LogInformation("Revenue event recorded for campaign {CampaignId}, variant {VariantId}", campaignId, variantId);
    }

    public async Task<decimal> CalculateProfitLiftAsync(Guid campaignId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var revenues = await _revenueRepository.GetByCampaignIdAsync(campaignId);
        var filteredRevenues = revenues.AsQueryable();

        if (startDate.HasValue)
        {
            filteredRevenues = filteredRevenues.Where(r => r.CreatedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            filteredRevenues = filteredRevenues.Where(r => r.CreatedAt <= endDate.Value);
        }

        var controlRevenues = filteredRevenues.Where(r => r.IsControl);
        var controlRevenue = controlRevenues.Sum(r => r.Amount);
        var controlConversions = controlRevenues.Count();

        var variantRevenues = filteredRevenues.Where(r => !r.IsControl);
        var variantRevenue = variantRevenues.Sum(r => r.Amount);
        var variantConversions = variantRevenues.Count();

        if (controlConversions == 0 || variantConversions == 0)
        {
            return 0; // Or handle as an error/special case
        }

        var controlRPC = controlRevenue / controlConversions;
        var variantRPC = variantRevenue / variantConversions;

        if (controlRPC == 0)
        {
            return variantRPC > 0 ? decimal.MaxValue : 0;
        }

        return (variantRPC - controlRPC) / controlRPC * 100;
     }

    private RevenueMetrics CalculateRevenueMetrics(IEnumerable<Revenue> revenueEvents)
    {
        var eventsList = revenueEvents.ToList();

        var totalRevenue = eventsList.Sum(e => e.Amount);
        var totalConversions = eventsList.Count;
        var averageOrderValue = totalConversions > 0 ? totalRevenue / totalConversions : 0;

        var revenueByVariant = eventsList
            .GroupBy(e => e.IsControl ? "Control" : e.VariantId.ToString())
            .ToDictionary(
                g => g.Key!,
                g => g.Sum(e => e.Amount)
            );

        return new RevenueMetrics
        {
            TotalRevenue = totalRevenue,
            NumberOfConversions = totalConversions,
            AverageOrderValue = averageOrderValue,
            RevenueByVariant = revenueByVariant.ToDictionary(
                kvp => Guid.TryParse(kvp.Key, out Guid guid) ? guid : Guid.Empty,
                kvp => kvp.Value
            )
        };
    }

        public async Task<decimal> CalculateROIAsync(Guid campaignId)
        {
            var revenueEvents = await _revenueRepository.GetByCampaignIdAsync(campaignId);
            if (revenueEvents == null || !revenueEvents.Any())
            {
                return 0;
            }

            var controlVariant = revenueEvents.FirstOrDefault(e => e.IsControl);
            if (controlVariant == null)
            {
                // Or handle as an error
                return 0;
            }

            var controlRevenue = revenueEvents.Where(e => e.IsControl).Sum(e => e.Amount);
            var controlConversions = revenueEvents.Count(e => e.IsControl);

            var variantRevenue = revenueEvents.Where(e => !e.IsControl).Sum(e => e.Amount);
            var variantConversions = revenueEvents.Count(e => !e.IsControl);

            if (controlConversions == 0) return variantConversions == 0 ? 0 : decimal.MaxValue;

            var controlRPC = controlRevenue / controlConversions;
            var variantRPC = variantConversions == 0 ? 0 : variantRevenue / variantConversions;

            if (controlRPC == 0) return variantRPC == 0 ? 0 : decimal.MaxValue;

            var roi = (variantRPC - controlRPC) / controlRPC * 100;
            return Math.Round(roi, 2);
        }

    public Task<Dictionary<Guid, decimal>> GetRevenuePerVisitorAsync(Guid campaignId)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<Guid, decimal>> GetAverageOrderValueAsync(Guid campaignId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RevenueTrendPoint>> GetRevenueTrendsAsync(Guid campaignId, DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> CalculateLifetimeValueImpactAsync(Guid campaignId)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, decimal>> GetProfitByTrafficSourceAsync(Guid campaignId)
    {
        // Phase 2
        throw new NotImplementedException();
    }

    public async Task<RevenueReport> GenerateRevenueReportAsync(Guid campaignId, DateTime startDate, DateTime endDate)
    {
        var revenueEvents = await _revenueRepository.GetByCampaignIdAsync(campaignId);
        var filteredEvents = revenueEvents.Where(e => e.RevenueDate >= startDate && e.RevenueDate <= endDate).ToList();

        var metrics = CalculateRevenueMetrics(filteredEvents);

        var controlDataKvp = metrics.RevenueByVariant.FirstOrDefault(v => v.Key == Guid.Empty);
        var variantData = metrics.RevenueByVariant.Where(v => v.Key != Guid.Empty).ToList();

        decimal overallMcp = 0;
        string winningVariant = "N/A";

        if (!controlDataKvp.Equals(default(KeyValuePair<Guid, decimal>)) && variantData.Any())
        {
            var controlRevenue = controlDataKvp.Value;
            var controlConversions = filteredEvents.Count(e => e.IsControl);

            var bestVariant = variantData.OrderByDescending(v => v.Value).First();
            var variantRevenue = bestVariant.Value;
            var variantConversions = filteredEvents.Count(e => !e.IsControl && e.VariantId == bestVariant.Key);

            if (controlConversions > 0 && variantConversions > 0)
            {
                overallMcp = CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
                if (overallMcp > 0)
                {
                    winningVariant = bestVariant.Key.ToString();
                }
                else
                {
                    winningVariant = "Control";
                }
            }
        }

        var reportSource = new RevenueReportSource
        {
            CampaignId = campaignId,
            StartDate = startDate,
            EndDate = endDate,
            Metrics = metrics,
            OverallMcp = overallMcp,
            WinningVariant = winningVariant
        };

        return _reportMapper.Map(reportSource);
    }

    private decimal CalculateMCP(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
    {
        if (controlConversions == 0 || variantConversions == 0)
        {
            return 0;
        }

        var controlRpc = controlRevenue / controlConversions;
        var variantRpc = variantRevenue / variantConversions;

        if (controlRpc == 0)
        {
            return variantRpc > 0 ? decimal.MaxValue : 0;
        }

        var mcp = ((variantRpc - controlRpc) / controlRpc) * 100;
        return Math.Round(mcp, 2);
    }

    public Task<RevenueMetrics> GetRevenueMetricsAsync(Guid campaignId)
    {
        throw new NotImplementedException();
    }
}