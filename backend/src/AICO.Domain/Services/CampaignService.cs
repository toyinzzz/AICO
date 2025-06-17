using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;

namespace AICO.Domain.Services
{
    public class CampaignService : ICampaignService
    {
        public Task<Campaign?> CreateCampaignAsync(Guid websiteId, string name, string targetUrl, string? description = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCampaignAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign?> GetCampaignByIdAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Campaign>?> GetCampaignsByWebsiteAsync(Guid websiteId)
        {
            throw new NotImplementedException();
        }

        public Task<CampaignSummary?> GetCampaignSummaryAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign?> PauseCampaignAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign?> StartCampaignAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign?> StopCampaignAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public Task<Campaign?> UpdateCampaignAsync(Guid campaignId, string name, string? description, string targetUrl)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateOwnershipAsync(Guid campaignId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}