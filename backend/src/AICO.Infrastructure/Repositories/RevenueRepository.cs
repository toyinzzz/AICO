using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories;

public class RevenueRepository : BaseRepository<Revenue>, IRevenueRepository
{
    public RevenueRepository(AicoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Revenue>> GetByCampaignIdAsync(Guid campaignId)
    {
        return await _context.Revenues
            .Where(r => r.CampaignId == campaignId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueByCampaignIdAsync(Guid campaignId)
    {
        return await _context.Revenues
            .Where(r => r.CampaignId == campaignId)
            .SumAsync(r => r.Amount);
    }

    public async Task<IEnumerable<Revenue>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Revenues
            .Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public Task<IEnumerable<Revenue>> GetByAbTestIdAsync(Guid abTestId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Revenue>> GetByVariantIdAsync(Guid variantId)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetTotalRevenueAsync(Guid campaignId)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetTotalRevenueByVariantAsync(Guid variantId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Revenue>> GetByWebsiteIdAsync(Guid websiteId)
    {
        throw new NotImplementedException();
    }

    Task IRepository<Revenue>.UpdateAsync(Revenue entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}