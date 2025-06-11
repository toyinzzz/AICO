using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Queries
{
    public interface ICampaignQueryHandler
    {
        Task<Campaign?> GetCampaignByIdAsync(Guid id);
        Task<IEnumerable<Campaign>> GetCampaignsByWebsiteIdAsync(Guid websiteId);
        Task<IEnumerable<Campaign>> GetActiveCampaignsAsync();
        Task<IEnumerable<Campaign>> GetCampaignsByStatusAsync(string status);
        Task<IEnumerable<Campaign>> GetCampaignsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}