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
                .Where(m => m.MetricType == metricType)
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
                .Where(m => m.WebsiteId == websiteId && m.MetricType == metricType)
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public Task<IEnumerable<Metric>> GetByCategoryAsync(Guid websiteId, string category, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Metric>> GetByNameAsync(Guid websiteId, string name, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Metric>> GetByDimensionAsync(Guid websiteId, string dimension, string dimensionValue = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<Metric> GetLatestAsync(Guid websiteId, string name, string dimension = null, string dimensionValue = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Metric>> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<(DateTime Date, double Value)>> GetTrendAsync(Guid websiteId, string name, string timeframe, string dimension = null, string dimensionValue = null)
        {
            throw new NotImplementedException();
        }

        public Task BulkInsertAsync(IEnumerable<Metric> metrics)
        {
            throw new NotImplementedException();
        }

        Task IRepository<Metric>.UpdateAsync(Metric entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}