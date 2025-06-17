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

        public async Task<Website?> GetByUrlAsync(string url)
        {
            // Assuming URL might be stored in a specific field or related to the domain.
            // For now, let's assume the domain is part of the URL or the URL is the domain.
            // This might need a more sophisticated way to parse the domain from the URL.
            return await _dbSet.FirstOrDefaultAsync(w => w.Domain == new Uri(url).Host || w.Domain == url);
        }

        public async Task<bool> IsUrlInUseAsync(string url)
        {
            // Similar assumption as GetByUrlAsync
            return await _dbSet.AnyAsync(w => w.Domain == new Uri(url).Host || w.Domain == url);
        }

        public async Task<IEnumerable<Website>> GetByIndustryAsync(string industry)
        {
            // Assuming Website entity has an 'Industry' property
            return await _dbSet.Where(w => w.Industry == industry).ToListAsync();
        }

        Task IRepository<Website>.UpdateAsync(Website entity)
        {
            return UpdateAsync(entity);
        }
    }
}