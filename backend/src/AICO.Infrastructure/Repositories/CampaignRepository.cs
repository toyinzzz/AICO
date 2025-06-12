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

        public async Task<IEnumerable<Campaign>> GetByStatusAsync(CampaignStatus status)
        {
            return await _dbSet
                .Where(c => c.Status == status)
                .Include(c => c.AbTests)
                .ToListAsync();
        }

        public Task<IEnumerable<Campaign>> GetByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Campaign>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        Task IRepository<Campaign>.UpdateAsync(Campaign entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}