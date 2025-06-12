using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories;

public class AnalysisResultRepository : BaseRepository<AnalysisResult>, IAnalysisResultRepository
{
    public AnalysisResultRepository(AicoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AnalysisResult>> GetByWebsiteIdAsync(Guid websiteId)
    {
        return await _context.AnalysisResults
            .Where(ar => ar.WebsiteId == websiteId)
            .OrderByDescending(ar => ar.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AnalysisResult>> GetByStatusAsync(string status)
    {
        return await _context.AnalysisResults
            .Where(ar => ar.Status == status)
            .OrderByDescending(ar => ar.CreatedAt)
            .ToListAsync();
    }

    public async Task<AnalysisResult?> GetLatestByWebsiteIdAsync(Guid websiteId)
    {
        return await _context.AnalysisResults
            .Where(ar => ar.WebsiteId == websiteId)
            .OrderByDescending(ar => ar.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<AnalysisResult>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.AnalysisResults
            .Where(ar => ar.CreatedAt >= startDate && ar.CreatedAt <= endDate)
            .OrderByDescending(ar => ar.CreatedAt)
            .ToListAsync();
    }

    public Task<IEnumerable<AnalysisResult>> GetByTypeAndWebsiteIdAsync(string analysisType, Guid websiteId)
    {
        throw new NotImplementedException();
    }

    Task IRepository<AnalysisResult>.UpdateAsync(AnalysisResult entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}