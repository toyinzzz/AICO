using AICO.Domain.DTOs;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service interface for Campaign management and A/B testing campaigns
    /// </summary>
    public interface ICampaignService
    {
        /// <summary>
        /// Creates a new A/B testing campaign
        /// </summary>
        Task<Campaign?> CreateCampaignAsync(Guid websiteId, Guid userId, string name, string targetUrl, string? description = null);

        /// <summary>
        /// Gets a campaign by ID
        /// </summary>
        Task<Campaign?> GetCampaignByIdAsync(Guid campaignId);

        /// <summary>
        /// Gets all campaigns for a website
        /// </summary>
        Task<IEnumerable<Campaign>?> GetCampaignsByWebsiteAsync(Guid websiteId);

        /// <summary>
        /// Updates campaign configuration
        /// </summary>
        Task<Campaign?> UpdateCampaignAsync(Guid campaignId, string name, string? description, string targetUrl);

        /// <summary>
        /// Starts a campaign (begins A/B testing)
        /// </summary>
        Task<Campaign?> StartCampaignAsync(Guid campaignId);

        /// <summary>
        /// Stops a campaign
        /// </summary>
        Task<Campaign?> StopCampaignAsync(Guid campaignId);

        /// <summary>
        /// Pauses a campaign temporarily
        /// </summary>
        Task<Campaign?> PauseCampaignAsync(Guid campaignId);

        /// <summary>
        /// Deletes a campaign and all associated data
        /// </summary>
        Task DeleteCampaignAsync(Guid campaignId);

        /// <summary>
        /// Gets campaign performance summary
        /// </summary>
        Task<CampaignSummary?> GetCampaignSummaryAsync(Guid campaignId);

        /// <summary>
        /// Validates user ownership of campaign
        /// </summary>
        Task<bool> ValidateOwnershipAsync(Guid campaignId, Guid userId);
    }
}