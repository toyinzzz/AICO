using AICO.Domain.DTOs;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AICO.Application.Services
{
    public class ProfitTrackingService : IProfitTrackingService
    {
        private const int MinimumSampleSize = 30;
        private readonly IRevenueRepository _revenueRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebsiteService _websiteService;

        public ProfitTrackingService(
            IRevenueRepository revenueRepository,
            IHttpContextAccessor httpContextAccessor,
            IWebsiteService websiteService)
        {
            _revenueRepository = revenueRepository;
            _httpContextAccessor = httpContextAccessor;
            _websiteService = websiteService;
        }

        public decimal CalculateMCP(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            ValidateInputs(controlRevenue, controlConversions, variantRevenue, variantConversions);
            var controlRPC = controlConversions == 0 ? 0 : controlRevenue / controlConversions;
            var variantRPC = variantConversions == 0 ? 0 : variantRevenue / variantConversions;
            if (controlRPC == 0) return variantRPC == 0 ? 0 : decimal.MaxValue;
            var mcp = ((variantRPC - controlRPC) / controlRPC) * 100;
            return Math.Round(mcp, 4);
        }

        public decimal CalculateMCPWithCurrency(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, string currency)
        {
            // Currency normalization should be handled upstream or injected as a service if needed
            return CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
        }

        public List<MCPResult> CalculateMCPForMultipleVariants(List<VariantData> variants)
        {
            var controlVariant = variants.FirstOrDefault(v => v.IsControl);
            if (controlVariant == null)
                throw new ArgumentException("Control variant not found");
            var results = new List<MCPResult>();
            foreach (var variant in variants.Where(v => !v.IsControl))
            {
                var mcp = CalculateMCP(controlVariant.Revenue, controlVariant.Conversions, variant.Revenue, variant.Conversions);
                var isSignificant = variant.Conversions >= MinimumSampleSize && controlVariant.Conversions >= MinimumSampleSize;
                results.Add(new MCPResult
                {
                    VariantId = variant.VariantId,
                    MCP = mcp,
                    IsStatisticallySignificant = isSignificant,
                    SampleSize = variant.Conversions
                });
            }
            return results;
        }

        public decimal? CalculateMCPWithSignificance(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            if (controlConversions < MinimumSampleSize || variantConversions < MinimumSampleSize)
                return null;
            return CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
        }

        public async Task<Revenue> ProcessStripeWebhookAsync(StripeWebhookEvent stripeEvent)
        {
            var sessionId = stripeEvent.Data.Metadata?.GetValueOrDefault("session_id");
            var variantId = stripeEvent.Data.Metadata?.GetValueOrDefault("variant_id");
            var websiteIdStr = stripeEvent.Data.Metadata?.GetValueOrDefault("website_id");
            var userIdStr = stripeEvent.Data.Metadata?.GetValueOrDefault("user_id");
            var amount = stripeEvent.Data.Amount;
            var currency = stripeEvent.Data.Currency ?? "USD";
            
            var websiteId = GetWebsiteIdFromContext(websiteIdStr);
            var userId = GetUserIdFromContext(userIdStr);

            // Ownership-Check: Use the interface-compliant ValidateOwnershipAsync
            if (websiteId != Guid.Empty && userId != Guid.Empty)
            {
                var isOwner = await _websiteService.ValidateOwnershipAsync(websiteId, userId);
                if (!isOwner)
                    throw new UnauthorizedAccessException($"User {userId} does not own website {websiteId}");
            }

            // FIXED: Better GUID parsing with proper error handling
            Guid? sessionGuid = null;
            if (!string.IsNullOrEmpty(sessionId))
            {
                if (Guid.TryParse(sessionId, out var parsedSessionId))
                    sessionGuid = parsedSessionId;
                else
                    throw new ArgumentException($"Invalid session_id format: {sessionId}");
            }

            Guid? variantGuid = null;
            if (!string.IsNullOrEmpty(variantId))
            {
                if (Guid.TryParse(variantId, out var parsedVariantId))
                    variantGuid = parsedVariantId;
                else
                    throw new ArgumentException($"Invalid variant_id format: {variantId}");
            }

            var revenue = new Revenue(
                abTestId: Guid.Empty,
                websiteId: websiteId,
                websiteId: websiteId,
                userId: userId,
                amount: amount / 100m,
                source: "stripe",
                currency: currency,
                transactionId: stripeEvent.Data.Id,
                sessionId: sessionGuid,
                variantId: variantGuid
            );
            
            await _revenueRepository.AddAsync(revenue);
            return revenue;
        }

        public async Task<RevenueReport> GenerateRevenueReportAsync(Guid abTestId, DateTime startDate, DateTime endDate)
        {
            var revenues = await _revenueRepository.GetByAbTestIdAsync(abTestId);
            var filteredRevenues = revenues.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate);
            var totalRevenue = filteredRevenues.Sum(r => r.Amount);
            var totalConversions = filteredRevenues.Count();
            var averageOrderValue = totalConversions > 0 ? totalRevenue / totalConversions : 0;

            // FIXED: Proper handling of nullable Guid in GroupBy
            var revenueByVariant = filteredRevenues
                .GroupBy(r => r.VariantId ?? Guid.Empty)
                .ToDictionary(g => g.Key, g => g.Sum(r => r.Amount));

            return new RevenueReport
            {
                AbTestId = abTestId,
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = totalRevenue,
                TotalConversions = totalConversions,
                AverageOrderValue = averageOrderValue,
                RevenueByVariant = revenueByVariant
            };
        }

        private Guid GetWebsiteIdFromContext(string websiteIdStr)
        {
            // Try to parse from metadata first
            if (!string.IsNullOrEmpty(websiteIdStr) && Guid.TryParse(websiteIdStr, out var websiteIdFromMetadata))
                return websiteIdFromMetadata;
            
            // Fall back to claims
            var context = _httpContextAccessor.HttpContext;
            if (context?.User?.Identity?.IsAuthenticated == true)
            {
                var websiteIdClaim = context.User.FindFirst("website_id")?.Value;
                if (!string.IsNullOrEmpty(websiteIdClaim) && Guid.TryParse(websiteIdClaim, out var websiteIdFromClaim))
                    return websiteIdFromClaim;
            }
            
            return Guid.Empty;
        }

        private Guid GetUserIdFromContext(string userIdStr)
        {
            // Try to parse from metadata first
            if (!string.IsNullOrEmpty(userIdStr) && Guid.TryParse(userIdStr, out var userIdFromMetadata))
                return userIdFromMetadata;
            
            // Fall back to claims
            var context = _httpContextAccessor.HttpContext;
            if (context?.User?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userIdFromClaim))
                    return userIdFromClaim;
            }
            
            return Guid.Empty;
        }

        public decimal CalculateROI(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            ValidateInputs(controlRevenue, controlConversions, variantRevenue, variantConversions);
            var controlRPC = controlConversions == 0 ? 0 : controlRevenue / controlConversions;
            var variantRPC = variantConversions == 0 ? 0 : variantRevenue / variantConversions;
            if (controlRPC == 0) return variantRPC == 0 ? 0 : decimal.MaxValue;
            var roi = ((variantRPC - controlRPC) / controlRPC) * 100;
            return Math.Round(roi, 2);
        }

        private static void ValidateInputs(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            if (controlConversions <= 0 || variantConversions <= 0)
                throw new ArgumentException("Conversions must be greater than zero");
            if (controlRevenue < 0 || variantRevenue < 0)
                throw new ArgumentException("Revenue cannot be negative");
        }
    }
}