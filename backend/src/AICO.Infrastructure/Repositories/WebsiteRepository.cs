using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories
{
    public class WebsiteRepository : BaseRepository<Website>, IWebsiteRepository
    {
        public WebsiteRepository(AicoDbContext context) : base(context)
        {
        }

        public async Task<Website?> GetByDomainAsync(string domain)
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.Domain == domain);
        }

        public async Task<IEnumerable<Website>> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> ExistsByDomainAsync(string domain)
        {
            return await _dbSet.AnyAsync(w => w.Domain == domain);
        }

        public Task<Website> GetByUrlAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUrlInUseAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Website>> GetByIndustryAsync(string industry)
        {
            throw new NotImplementedException();
        }

        Task IRepository<Website>.UpdateAsync(Website entity)
        {
            return UpdateAsync(entity);
        }

        public Task DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}