using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories;

public class SnippetRepository : BaseRepository<Snippet>, ISnippetRepository
{
    public SnippetRepository(AicoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Snippet>> GetByWebsiteIdAsync(Guid websiteId)
    {
        return await _context.Snippets
            .Where(s => s.WebsiteId == websiteId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Snippet>> GetActiveSnippetsAsync(Guid websiteId)
    {
        return await _context.Snippets
            .Where(s => s.WebsiteId == websiteId && s.IsActive)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Snippet?> GetByCodeAsync(string code)
    {
        return await _context.Snippets
            .FirstOrDefaultAsync(s => s.Code == code);
    }

    public Task<IEnumerable<Snippet>> GetByCampaignIdAsync(Guid campaignId)
    {
        throw new NotImplementedException();
    }

    public Task<Snippet?> GetActiveSnippetAsync(Guid websiteId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Snippet>> GetByStatusAsync(string status)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    Task IRepository<Snippet>.UpdateAsync(Snippet entity)
    {
        throw new NotImplementedException();
    }

    public override Task DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}