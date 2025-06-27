using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Website entity operations
    /// </summary>
    public interface IWebsiteRepository : IRepository<Website>
    {
        /// <summary>
        /// Gets all websites for a specific user
        /// </summary>
        Task<IEnumerable<Website>> GetByUserIdAsync(Guid userId);

        /// <summary>
        /// Gets a website by URL
        /// </summary>
        Task<Website?> GetByUrlAsync(string url);

        /// <summary>
        /// Checks if a URL is already in use
        /// </summary>
        Task<bool> IsUrlInUseAsync(string url);

        /// <summary>
        /// Gets websites by industry
        /// </summary>
        Task<IEnumerable<Website>> GetByIndustryAsync(string industry);
        Task<IEnumerable<Website>> GetByOwnerIdAsync(Guid userId);
    }
}