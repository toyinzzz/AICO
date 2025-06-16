using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories
{
    public class VariantRepository : BaseRepository<Variant>, IVariantRepository
    {
        public VariantRepository(AicoDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid abTestId)
        {
            return await _dbSet.AnyAsync(v => v.Name == name && v.AbTestId == abTestId);
        }

        public async Task<IEnumerable<Variant>> GetByAbTestIdAsync(Guid abTestId)
        {
            return await _dbSet
                .Where(v => v.AbTestId == abTestId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Variant>> GetByCampaignIdAsync(Guid campaignId)
        {
            return await _dbSet
                .Include(v => v.AbTest)
                .Where(v => v.AbTest.CampaignId == campaignId)
                .ToListAsync();
        }

        public async Task<Variant?> GetByNameAsync(string name, Guid abTestId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(v => v.Name == name && v.AbTestId == abTestId);
        }

        public async Task<Variant?> GetControlVariantAsync(Guid abTestId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(v => v.AbTestId == abTestId && v.IsControl);
        }

        public async Task<IEnumerable<Variant>> GetTestVariantsAsync(Guid abTestId)
        {
            return await _dbSet
                .Where(v => v.AbTestId == abTestId && !v.IsControl)
                .ToListAsync();
        }
    }
}