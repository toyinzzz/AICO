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

    public async Task<IEnumerable<Conversion>> GetConversionsByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId);
        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);
        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Conversion>> GetConversionsBySessionIdAsync(Guid sessionId)
    {
        return await _context.Conversions
            .Where(c => c.SessionId == sessionId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Conversion>> GetConversionsByTypeAsync(Guid websiteId, string conversionType, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId && c.ConversionType.ToString() == conversionType);
        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);
        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Conversion>> GetConversionsByGoalAsync(Guid websiteId, string goalName, DateTime? startDate = null, DateTime? endDate = null)
    {
        // Assuming GoalName is a property on Conversion entity
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId && c.GoalName == goalName);
        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);
        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<(DateTime Date, int Count, decimal? Value)>> GetConversionTrendsAsync(Guid websiteId, string timeframe, string? goalName = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Conversions.Where(c => c.WebsiteId == websiteId);

        if (!string.IsNullOrEmpty(goalName))
            query = query.Where(c => c.GoalName == goalName);

        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);

        // Group by day or week
        var groupedQuery = timeframe.ToLower() == "week"
            ? query.GroupBy(c => EF.Functions.DateFromParts(
                c.CreatedAt.Year,
                c.CreatedAt.Month,
                c.CreatedAt.Day - ((int)c.CreatedAt.DayOfWeek)))
            : query.GroupBy(c => EF.Functions.DateFromParts(c.CreatedAt.Year, c.CreatedAt.Month, c.CreatedAt.Day));

        // Aggregate the data
        var trends = await groupedQuery
            .Select(g => new
            {
                Date = g.Key,
                Count = g.Count(),
                Value = g.Sum(c => c.Value)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        // Convert to tuple list
        return trends.Select(t => (t.Date, t.Count, t.Value)).ToList();
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



    public async Task BulkInsertAsync(IEnumerable<Conversion> conversions)
    {
        await _context.Conversions.AddRangeAsync(conversions);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<(DateTime Date, int Count, decimal? Value)>?> GetTrendsAsync(Guid websiteId, string timeframe, string? goalName = null)
    {
        throw new NotImplementedException();
    }
}