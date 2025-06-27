using AICO.Domain.DTOs;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service interface for JavaScript snippet generation and management
    /// </summary>
    public interface ISnippetService
    {
        /// <summary>
        /// Generates JavaScript snippet for website integration
        /// </summary>
        Task<string> GenerateSnippetAsync(Guid websiteId);

        /// <summary>
        /// Gets snippet configuration for a website
        /// </summary>
        Task<SnippetConfig> GetSnippetConfigAsync(Guid websiteId);

        /// <summary>
        /// Updates snippet configuration
        /// </summary>
        Task UpdateSnippetConfigAsync(Guid websiteId, SnippetConfigDto config);

        /// <summary>
        /// Validates snippet installation on website
        /// </summary>
        Task<bool> ValidateSnippetInstallationAsync(Guid websiteId);

        /// <summary>
        /// Gets snippet analytics and performance
        /// </summary>
        Task<SnippetAnalytics> GetSnippetAnalyticsAsync(Guid websiteId);

        /// <summary>
        /// Regenerates snippet with new configuration
        /// </summary>
        Task<string> RegenerateSnippetAsync(Guid websiteId);

        /// <summary>
        /// Gets snippet installation instructions
        /// </summary>
        Task<SnippetInstructions> GetInstallationInstructionsAsync(Guid websiteId);
    }
}