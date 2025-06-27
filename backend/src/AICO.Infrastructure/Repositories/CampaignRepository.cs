using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories
{
    public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(AicoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Campaign>> GetByWebsiteIdAsync(Guid websiteId)
        {
            return await _dbSet
                .Where(c => c.WebsiteId == websiteId)
                .Include(c => c.AbTests)
                .ToListAsync();
        }

        public async Task<IEnumerable<Campaign>> GetActiveCampaignsAsync()
        {
            return await _dbSet
                .Where(c => c.Status == CampaignStatus.Active)
                .Include(c => c.AbTests)
                .ToListAsync();
        }

        public async Task<Campaign?> GetByIdWithAbTestsAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.AbTests)
                .ThenInclude(a => a.Variants)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Campaign>> GetByStatusAsync(string status)
        {
            if (Enum.TryParse<CampaignStatus>(status, true, out var campaignStatus))
            {
                return await _dbSet
                    .Where(c => c.Status == campaignStatus)
                    .Include(c => c.AbTests)
                    .ToListAsync();
            }
            // Optionally, handle invalid status string, e.g., return empty list or throw exception
            return Enumerable.Empty<Campaign>();
        }

        public async Task<Campaign?> GetByNameAsync(string name)
        {
            return await _dbSet
                .Where(c => c.Name == name)
                .Include(c => c.AbTests)
                .ThenInclude(a => a.Variants) // Keep includes if relevant for a campaign fetched by name
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dbSet.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Campaign>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(c => c.StartDate.HasValue && c.StartDate.Value <= endDate && 
                             (c.EndDate.HasValue ? c.EndDate.Value >= startDate : true))
                .Include(c => c.AbTests)
                .ToListAsync();
        }
    }
}