using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service interface for Website-related business logic
    /// </summary>
    public interface IWebsiteService
    {
        /// <summary>
        /// Creates a new website
        /// </summary>
        Task<Website> CreateAsync(string url, string name, string description, string industry, Guid userId);
        
        /// <summary>
        /// Gets a website by ID
        /// </summary>
        Task<Website> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Gets all websites for a specific user
        /// </summary>
        Task<IEnumerable<Website>> GetByUserIdAsync(Guid userId);
        
        /// <summary>
        /// Updates a website's information
        /// </summary>
        Task<Website> UpdateAsync(Guid id, string name, string description, string industry);
        
        /// <summary>
        /// Updates a website's URL
        /// </summary>
        Task<Website> UpdateUrlAsync(Guid id, string url);
        
        /// <summary>
        /// Deletes a website
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Validates that a user has permission to access a website
        /// </summary>
        Task<bool> ValidateOwnershipAsync(Guid websiteId, Guid userId);
    }
} 