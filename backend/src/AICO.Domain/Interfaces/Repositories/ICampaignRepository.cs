using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Domain.Interfaces.Repositories
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
        Task<IEnumerable<Campaign>> GetByWebsiteIdAsync(Guid websiteId);
        Task<IEnumerable<Campaign>> GetByStatusAsync(string status);
        Task<IEnumerable<Campaign>> GetActiveCampaignsAsync();
        Task<Campaign?> GetByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name);
        Task<IEnumerable<Campaign>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}