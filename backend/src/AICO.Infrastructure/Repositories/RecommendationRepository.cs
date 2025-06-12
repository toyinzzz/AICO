using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories;

public class RecommendationRepository : BaseRepository<Recommendation>, IRecommendationRepository
{
    public RecommendationRepository(AicoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Recommendation>> GetByAnalysisResultIdAsync(Guid analysisResultId)
    {
        return await _context.Recommendations
            .Where(r => r.AnalysisResultId == analysisResultId)
            .OrderByDescending(r => r.Priority)
            .ThenByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommendation>> GetByStatusAsync(RecommendationStatus status)
    {
        return await _context.Recommendations
            .Where(r => r.Status == status)
            .OrderByDescending(r => r.Priority)
            .ThenByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommendation>> GetByPriorityAsync(int minPriority)
    {
        return await _context.Recommendations
            .Where(r => r.Priority >= minPriority)
            .OrderByDescending(r => r.Priority)
            .ThenByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommendation>> GetImplementedRecommendationsAsync()
    {
        return await _context.Recommendations
            .Where(r => r.Status == RecommendationStatus.Implemented)
            .OrderByDescending(r => r.ImplementedAt)
            .ToListAsync();
    }

    public Task<IEnumerable<Recommendation>> GetByCategoryAndAnalysisResultIdAsync(string category, Guid analysisResultId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Recommendation>> GetByImplementationStatusAndAnalysisResultIdAsync(bool isImplemented, Guid analysisResultId)
    {
        throw new NotImplementedException();
    }

    Task IRepository<Recommendation>.UpdateAsync(Recommendation entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}