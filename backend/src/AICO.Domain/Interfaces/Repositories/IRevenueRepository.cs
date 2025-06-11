using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Domain.Interfaces.Repositories
{
    public interface IRevenueRepository : IRepository<Revenue>
    {
        Task<IEnumerable<Revenue>> GetByCampaignIdAsync(Guid campaignId);
        Task<IEnumerable<Revenue>> GetByAbTestIdAsync(Guid abTestId);
        Task<IEnumerable<Revenue>> GetByVariantIdAsync(Guid variantId);
        Task<IEnumerable<Revenue>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalRevenueAsync(Guid campaignId);
        Task<decimal> GetTotalRevenueByVariantAsync(Guid variantId);
        Task<IEnumerable<Revenue>> GetByWebsiteIdAsync(Guid websiteId);
    }
}