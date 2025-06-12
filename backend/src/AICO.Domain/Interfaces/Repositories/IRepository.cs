using AICO.Domain.Entities;
using System.Linq.Expressions;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="T">Entity type that derives from BaseEntity</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets entity by id
        /// </summary>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Finds entities based on predicate
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Removes an entity
        /// </summary>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Removes an entity by id
        /// </summary>
        Task DeleteByIdAsync(Guid id);

        /// <summary>
        /// Checks if any entity satisfies the predicate
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets or sets the URL
        /// </summary>
        public string? Url { get; set; }
    }
}