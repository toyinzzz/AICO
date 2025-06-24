using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories
{
    public class AbTestRepository : BaseRepository<AbTest>, IAbTestRepository
    {
        public AbTestRepository(AicoDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<AbTest>> GetByWebsiteIdAsync(Guid websiteId)
        {
            return await _dbSet
                .Where(a => a.WebsiteId == websiteId)
                .Include(a => a.Variants)
                .ToListAsync();
        }

        public async Task<IEnumerable<AbTest>> GetActiveAsync() // Renamed from GetActiveTestsAsync
        {
            return await _dbSet
                .Where(a => a.Status == AbTestStatus.Running) // Assuming Active means Running
                .Include(a => a.Variants)
                .ToListAsync();
        }

        public async Task<AbTest?> GetByIdWithVariantsAsync(Guid id)
        {
            return await _dbSet
                .Include(a => a.Variants)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<AbTest>> GetByStatusAsync(AbTestStatus status) // Changed string to AbTestStatus
        {
            return await _dbSet
                .Where(a => a.Status == status)
                .Include(a => a.Variants)
                .ToListAsync();
        }

        public async Task<bool> HasActiveTestForElementAsync(Guid websiteId, string elementSelector)
        {
            return await _dbSet.AnyAsync(a =>
                a.WebsiteId == websiteId &&
                a.TargetSelector == elementSelector &&
                a.Status == AbTestStatus.Running);
        }

        public async Task<IEnumerable<AbTest>> GetByCampaignIdAsync(Guid campaignId)
        {
            return await _dbSet.Where(a => a.CampaignId == campaignId).Include(a => a.Variants).ToListAsync();
        }

        // Removed redundant GetByStatusAsync(string status)

        public async Task<AbTest?> GetByNameAsync(string name)
        {
            return await _dbSet.Include(a => a.Variants).FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dbSet.AnyAsync(a => a.Name == name);
        }

        // Removed GetRunningTestsAsync and GetCompletedTestsAsync as they are covered by GetByStatusAsync(AbTestStatus status)
    }
}