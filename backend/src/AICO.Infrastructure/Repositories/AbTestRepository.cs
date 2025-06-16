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
                a.ElementSelector == elementSelector &&
                a.Status == AbTestStatus.Running);
        }
    }
}