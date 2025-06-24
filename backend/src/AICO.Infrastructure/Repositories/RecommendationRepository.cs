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

    public async Task<IEnumerable<Recommendation>> GetByStatusAsync(string status)
    {
        if (Enum.TryParse<RecommendationStatus>(status, true, out var statusEnum))
        {
            return await _context.Recommendations
                .Where(r => r.Status == statusEnum)
                .ToListAsync();
        }
        return new List<Recommendation>(); // Or throw an exception for invalid status
    }

    public async Task<IEnumerable<Recommendation>> GetByWebsiteIdAsync(Guid websiteId)
    {
        return await _context.Recommendations
            .Where(r => r.WebsiteId == websiteId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recommendation>> GetByAnalysisResultIdAsync(Guid analysisResultId)
    {
        return await _context.Recommendations
            .Where(r => r.AnalysisResultId == analysisResultId)
            .ToListAsync();
    }

    // Example of a more complex query method
    public async Task<IEnumerable<Recommendation>> GetPrioritizedRecommendationsAsync(Guid websiteId, int minPriority)
    {
        return await _context.Recommendations
            .Where(r => r.WebsiteId == websiteId && r.Priority >= minPriority)
            .OrderByDescending(r => r.Priority)
            .ThenByDescending(r => r.GeneratedAt)
            .ToListAsync();
    }

    public async Task<int> CountPendingRecommendationsAsync(Guid websiteId)
    {
        return await _context.Recommendations
            .CountAsync(r => r.WebsiteId == websiteId && r.Status == RecommendationStatus.Pending);
    }

    public async Task MarkAsImplementedAsync(Guid recommendationId)
    {
        var recommendation = await _context.Recommendations.FindAsync(recommendationId);
        if (recommendation != null)
        {
            recommendation.MarkAsImplemented();
            await _context.SaveChangesAsync();
        }
    }

    public async Task ChangeStatusAsync(Guid recommendationId, string newStatus)
    {
        var recommendation = await _context.Recommendations.FindAsync(recommendationId);
        if (recommendation != null && Enum.TryParse<RecommendationStatus>(newStatus, true, out var statusEnum))
        {
            recommendation.UpdateStatus(statusEnum);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Recommendation>> GetRecommendationsByCriteriaAsync(Guid websiteId, string status, int? priority, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Recommendations.Where(r => r.WebsiteId == websiteId);

        if (!string.IsNullOrEmpty(status))
        {
            if (Enum.TryParse<RecommendationStatus>(status, true, out var statusEnum))
            {
                query = query.Where(r => r.Status == statusEnum);
            }
            else
            {
                // Handle invalid status string, perhaps log a warning or throw an exception
                // For now, let's assume an invalid status string means no filtering by status
                // Or, if strict, throw new ArgumentException($"Invalid recommendation status: {status}");
            }
        }

        if (priority.HasValue)
        {
            query = query.Where(r => r.Priority == priority.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(r => r.GeneratedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.GeneratedAt <= endDate.Value);
        }

        return await query.OrderByDescending(r => r.GeneratedAt).ToListAsync();
    }

    public Task<IEnumerable<Recommendation>> GetByCategoryAndAnalysisResultIdAsync(string category, Guid analysisResultId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Recommendation>> GetByImplementationStatusAndAnalysisResultIdAsync(bool isImplemented, Guid analysisResultId)
    {
        throw new NotImplementedException();
    }
}