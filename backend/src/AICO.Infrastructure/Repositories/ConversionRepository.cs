using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories;

using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;



public class ConversionRepository : BaseRepository<Conversion>, IConversionRepository
{
    public ConversionRepository(AicoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Conversion>> GetByWebsiteIdAsync(Guid websiteId)
    {
        return await _context.Conversions
            .Where(c => c.WebsiteId == websiteId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Conversion>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Conversions
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}