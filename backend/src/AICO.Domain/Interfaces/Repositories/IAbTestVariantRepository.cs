using AICO.Domain.Entities;
using System.Linq.Expressions;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for managing A/B test variants.
    /// </summary>
    public interface IAbTestVariantRepository
    {
        Task<AbTestVariant?> GetByIdAsync(Guid id);
        Task<IEnumerable<AbTestVariant>> GetAllAsync();
        Task<IEnumerable<AbTestVariant>> FindAsync(Expression<Func<AbTestVariant, bool>> predicate);
        Task AddAsync(AbTestVariant entity);
        Task AddRangeAsync(IEnumerable<AbTestVariant> entities);
        void Update(AbTestVariant entity); // Typically synchronous for EF Core change tracking
        void Remove(AbTestVariant entity); // Typically synchronous for EF Core change tracking
        void RemoveRange(IEnumerable<AbTestVariant> entities); // Typically synchronous

        Task<IEnumerable<AbTestVariant>> GetVariantsByTestIdAsync(Guid abTestId);
        Task<AbTestVariant?> GetByNameAsync(Guid abTestId, string name);
        Task<int> GetTotalViewsForTestAsync(Guid abTestId);
        Task<int> GetTotalConversionsForTestAsync(Guid abTestId);
    }
}