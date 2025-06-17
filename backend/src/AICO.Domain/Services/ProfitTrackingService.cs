using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Domain.Services
{
    public class ProfitTrackingService : IProfitTrackingService
    {
        public decimal CalculateMCP(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            throw new NotImplementedException();
        }

        public decimal CalculateMCPWithCurrency(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, string currency)
        {
            throw new NotImplementedException();
        }

        public List<MCPResult> CalculateMCPForMultipleVariants(List<VariantData> variants)
        {
            throw new NotImplementedException();
        }

        public decimal? CalculateMCPWithSignificance(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            throw new NotImplementedException();
        }

        public Task<Revenue> ProcessStripeWebhookAsync(StripeWebhookEvent stripeEvent)
        {
            throw new NotImplementedException();
        }

        public decimal CalculateROI(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            throw new NotImplementedException();
        }

        public Task<RevenueReport> GenerateRevenueReportAsync(Guid abTestId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}