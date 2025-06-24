using AICO.Domain.DTOs;
using AICO.Domain.Entities;

namespace AICO.Application.Interfaces.Services
{
    public interface IProfitTrackingService
    {
        decimal CalculateMCP(decimal controlRevenue, int controlConversions, 
                           decimal variantRevenue, int variantConversions);
        decimal CalculateMCPWithCurrency(decimal controlRevenue, int controlConversions, 
                                       decimal variantRevenue, int variantConversions, string currency);
        List<MCPResult> CalculateMCPForMultipleVariants(List<VariantData> variants);
        decimal? CalculateMCPWithSignificance(decimal controlRevenue, int controlConversions, 
                                            decimal variantRevenue, int variantConversions);
        Task<Revenue> ProcessStripeWebhookAsync(StripeWebhookEvent stripeEvent);
        decimal CalculateROI(decimal controlRevenue, int controlConversions, 
                           decimal variantRevenue, int variantConversions);
        Task<RevenueReport> GenerateRevenueReportAsync(Guid abTestId, DateTime startDate, DateTime endDate);
    }

}