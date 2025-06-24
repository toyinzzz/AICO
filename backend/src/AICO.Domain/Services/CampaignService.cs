using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;

namespace AICO.Domain.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IWebsiteRepository _websiteRepository;

        public CampaignService(ICampaignRepository campaignRepository, IWebsiteRepository websiteRepository)
        {
            _campaignRepository = campaignRepository;
            _websiteRepository = websiteRepository;
        }

        public async Task<Campaign?> CreateCampaignAsync(Guid websiteId, Guid userId, string name, string targetUrl, string? description = null)
        {
            var website = await _websiteRepository.GetByIdAsync(websiteId);
            if (website == null)
            {
                // Or handle this case as per your application's requirements
                return null;
            }

            var campaign = new Campaign(name, description, websiteId, userId);

            await _campaignRepository.AddAsync(campaign);

            return campaign;
        }

        public async Task DeleteCampaignAsync(Guid campaignId)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign != null)
            {
                await _campaignRepository.DeleteAsync(campaign);
            }
        }

        public async Task<Campaign?> GetCampaignByIdAsync(Guid campaignId)
        {
            return await _campaignRepository.GetByIdAsync(campaignId);
        }

        public async Task<IEnumerable<Campaign>?> GetCampaignsByWebsiteAsync(Guid websiteId)
        {
            return await _campaignRepository.FindAsync(c => c.WebsiteId == websiteId);
        }

        public Task<CampaignSummary?> GetCampaignSummaryAsync(Guid campaignId)
        {
            // This would require a more complex query, likely involving other repositories
            // For now, returning a placeholder
            return Task.FromResult<CampaignSummary?>(new CampaignSummary());
        }

        public async Task<Campaign?> PauseCampaignAsync(Guid campaignId)
        {
            // Assuming 'Paused' is a status. If not, this needs adjustment.
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null) return null;

            // No 'Pause' method on entity, so this is a conceptual implementation
            // campaign.Pause(); 
            // await _campaignRepository.UpdateAsync(campaign);

            return campaign;
        }

        public async Task<Campaign?> StartCampaignAsync(Guid campaignId)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
            {
                return null;
            }

            campaign.Start(DateTime.UtcNow);
            await _campaignRepository.UpdateAsync(campaign);

            return campaign;
        }

        public async Task<Campaign?> StopCampaignAsync(Guid campaignId)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
            {
                return null;
            }

            campaign.Stop(DateTime.UtcNow);
            await _campaignRepository.UpdateAsync(campaign);

            return campaign;
        }

        public async Task<Campaign?> UpdateCampaignAsync(Guid campaignId, string name, string? description, string targetUrl)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
            {
                return null;
            }

            campaign.UpdateDetails(name, description ?? string.Empty);
            await _campaignRepository.UpdateAsync(campaign);

            return campaign;
        }

        public async Task<bool> ValidateOwnershipAsync(Guid campaignId, Guid userId)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            return campaign != null && campaign.UserId == userId;
        }
    }
}