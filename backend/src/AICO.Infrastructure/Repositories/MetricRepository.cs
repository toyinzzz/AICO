using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories
{
    public class MetricRepository : BaseRepository<Metric>, IMetricRepository
    {
        public MetricRepository(AicoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Metric>> GetByWebsiteIdAsync(Guid websiteId)
        {
            return await _dbSet
                .Where(m => m.WebsiteId == websiteId)
                .Include(m => m.Website)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Metric>> GetByMetricTypeAsync(string metricType)
        {
            return await _dbSet
                .Where(m => m.Category.Value == metricType)
                .Include(m => m.Website)
                .ToListAsync();
        }

        public async Task<IEnumerable<Metric>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(m => m.CreatedAt >= startDate && m.CreatedAt <= endDate)
                .Include(m => m.Website)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Metric?> GetLatestByTypeAsync(Guid websiteId, string metricType)
        {
            return await _dbSet
                .Where(m => m.WebsiteId == websiteId && m.Category.Value == metricType)
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Metric>?> GetByCategoryAsync(Guid websiteId, string category, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbSet.Where(m => m.WebsiteId == websiteId && m.Category.Value == category);
            if (startDate.HasValue)
                query = query.Where(m => m.CreatedAt >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(m => m.CreatedAt <= endDate.Value);
            return await query.OrderByDescending(m => m.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Metric>?> GetByNameAsync(Guid websiteId, string name, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbSet.Where(m => m.WebsiteId == websiteId && m.Name == name);
            if (startDate.HasValue)
                query = query.Where(m => m.CreatedAt >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(m => m.CreatedAt <= endDate.Value);
            return await query.OrderByDescending(m => m.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Metric>?> GetByDimensionAsync(Guid websiteId, string dimension, string? dimensionValue = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            // Assuming Dimension and DimensionValue are properties on Metric or a related entity.
            // This is a simplified version. Real implementation might need more complex querying based on how dimensions are stored.
            var query = _dbSet.Where(m => m.WebsiteId == websiteId /* && m.Dimension == dimension */ ); // Placeholder for actual dimension filtering
            // if (!string.IsNullOrEmpty(dimensionValue))
            // query = query.Where(m => m.DimensionValue == dimensionValue);
            if (startDate.HasValue)
                query = query.Where(m => m.CreatedAt >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(m => m.CreatedAt <= endDate.Value);
            return await query.OrderByDescending(m => m.CreatedAt).ToListAsync();
        }

        public async Task<Metric?> GetLatestAsync(Guid websiteId, string name, string? dimension = null, string? dimensionValue = null)
        {
            var query = _dbSet.Where(m => m.WebsiteId == websiteId && m.Name == name);
            // Add dimension filtering if applicable and properties exist
            // if (!string.IsNullOrEmpty(dimension) && !string.IsNullOrEmpty(dimensionValue))
            // query = query.Where(m => m.Dimension == dimension && m.DimensionValue == dimensionValue);
            return await query.OrderByDescending(m => m.CreatedAt).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Metric>?> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbSet.Where(m => m.WebsiteId == websiteId);
            if (startDate.HasValue)
                query = query.Where(m => m.CreatedAt >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(m => m.CreatedAt <= endDate.Value);
            return await query.Include(m => m.Website).OrderByDescending(m => m.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<(DateTime Date, double Value)>?> GetTrendAsync(Guid websiteId, string name, string timeframe, string? dimension = null, string? dimensionValue = null)
        {
            // Placeholder: Actual trend calculation would involve grouping and aggregation based on timeframe.
            // Returning empty list for now.
            return await Task.FromResult<IEnumerable<(DateTime Date, double Value)>?>(new List<(DateTime Date, double Value)>());
        }

        public async Task BulkInsertAsync(IEnumerable<Metric> metrics)
        {
            await _dbSet.AddRangeAsync(metrics);
            await _context.SaveChangesAsync();
        }

        async Task IRepository<Metric>.UpdateAsync(Metric entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public new async Task DeleteByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}