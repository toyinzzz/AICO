using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Generic repository interface for CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        Task<T> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();
        
        /// <summary>
        /// Finds entities based on a predicate
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
        /// Removes an entity by its ID
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Checks if any entity satisfies the given predicate
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
} 