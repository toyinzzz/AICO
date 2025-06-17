using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories;

using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
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
            .Include(c => c.Session) // Eagerly load the Session
            .Where(c => c.Session != null && c.Session.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Conversion>?> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId);
        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);
        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Conversion>?> GetBySessionIdAsync(Guid sessionId)
    {
        return await _context.Conversions
            .Where(c => c.SessionId == sessionId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Conversion>?> GetByTypeAsync(Guid websiteId, string conversionType, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId && c.ConversionType.ToString() == conversionType);
        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);
        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Conversion>?> GetByGoalAsync(Guid websiteId, string goalName, DateTime? startDate = null, DateTime? endDate = null)
    {
        // Assuming GoalName is a property on Conversion entity
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId && c.GoalName == goalName);
        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);
        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<int> GetCountAsync(Guid websiteId, string? goalName = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId);
        if (!string.IsNullOrEmpty(goalName))
            query = query.Where(c => c.GoalName == goalName);
        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);
        return await query.CountAsync();
    }

    public async Task<(decimal TotalValue, string? Currency)> GetTotalValueAsync(Guid websiteId, string? conversionType = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId && c.Value.HasValue);
        if (!string.IsNullOrEmpty(conversionType))
            query = query.Where(c => c.ConversionType.ToString() == conversionType);
        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);
        
        decimal totalValue = await query.SumAsync(c => c.Value ?? 0m);
        // Assuming all conversions for a website use the same currency or currency is stored per conversion.
        // For simplicity, picking the currency from the first conversion with a value, if any.
        string? currency = await query.Where(c => c.Value.HasValue && !string.IsNullOrEmpty(c.Currency))
                                     .Select(c => c.Currency)
                                     .FirstOrDefaultAsync();
        return (totalValue, currency);
    }

    public async Task<IEnumerable<(DateTime Date, int Count, decimal? Value)>?> GetTrendsAsync(Guid websiteId, string timeframe, string? goalName = null)
    {
        // Placeholder: Actual trend calculation would involve grouping by date based on timeframe.
        // Returning empty list for now.
        return await Task.FromResult<IEnumerable<(DateTime Date, int Count, decimal? Value)>?>(new List<(DateTime Date, int Count, decimal? Value)>());
    }

    public async Task BulkInsertAsync(IEnumerable<Conversion> conversions)
    {
        await _context.Conversions.AddRangeAsync(conversions);
        await _context.SaveChangesAsync();
    }
}