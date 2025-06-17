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

        public async Task<IEnumerable<AbTest>> GetActiveTestsAsync()
        {
            return await _dbSet
                .Where(a => a.Status == AbTestStatus.Running)
                .Include(a => a.Variants)
                .ToListAsync();
        }

        public async Task<AbTest?> GetByIdWithVariantsAsync(Guid id)
        {
            return await _dbSet
                .Include(a => a.Variants)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<AbTest>> GetByStatusAsync(AbTestStatus status)
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
            // Basic implementation: find tests by CampaignId. Assumes CampaignId is a property on AbTest entity.
            return await _dbSet.Where(a => a.CampaignId == campaignId).Include(a => a.Variants).ToListAsync();
        }

        public async Task<IEnumerable<AbTest>> GetByStatusAsync(string status)
        {
            // Basic implementation: find tests by status string. This might need parsing to AbTestStatus enum.
            // For now, returning empty list to avoid parsing errors.
            // Consider changing parameter to AbTestStatus enum if possible, or add robust parsing.
            if (Enum.TryParse<AbTestStatus>(status, true, out var parsedStatus))
            {
                return await _dbSet.Where(a => a.Status == parsedStatus).Include(a => a.Variants).ToListAsync();
            }
            return Enumerable.Empty<AbTest>();
        }

        public async Task<AbTest?> GetByNameAsync(string name)
        {
            // Basic implementation: find test by name. Assumes Name is a property on AbTest entity.
            return await _dbSet.Include(a => a.Variants).FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            // Basic implementation: check if test exists by name. Assumes Name is a property on AbTest entity.
            return await _dbSet.AnyAsync(a => a.Name == name);
        }

        public async Task<IEnumerable<AbTest>> GetRunningTestsAsync()
        {
            // Basic implementation: find running tests. This is similar to GetActiveTestsAsync.
            return await _dbSet.Where(a => a.Status == AbTestStatus.Running).Include(a => a.Variants).ToListAsync();
        }

        public async Task<IEnumerable<AbTest>> GetCompletedTestsAsync()
        {
            // Basic implementation: find completed tests.
            return await _dbSet.Where(a => a.Status == AbTestStatus.Completed).Include(a => a.Variants).ToListAsync();
        }
    }
}