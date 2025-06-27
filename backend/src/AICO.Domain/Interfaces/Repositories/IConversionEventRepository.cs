using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Domain.Interfaces.Repositories
{
    public interface IConversionEventRepository : IRepository<ConversionEvent>
    {
        Task<IEnumerable<ConversionEvent>> GetByCampaignIdAsync(Guid campaignId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<ConversionEvent>> GetByAbTestIdAsync(Guid abTestId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<ConversionEvent>> GetByVariantIdAsync(Guid variantId, DateTime startDate, DateTime endDate);
        // Add other specific methods for ConversionEvent as needed, for example:
        // Task<int> GetCountByVariantIdAsync(Guid variantId, DateTime startDate, DateTime endDate);
        // Task<decimal> CalculateConversionRateAsync(Guid variantId, DateTime startDate, DateTime endDate, int totalVisitors); // This might be better in a service layer
    }
}