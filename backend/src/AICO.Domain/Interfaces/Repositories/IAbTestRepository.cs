using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Domain.Interfaces.Repositories
{
    public interface IAbTestRepository : IRepository<AbTest>
    {
        Task<IEnumerable<AbTest>> GetByCampaignIdAsync(Guid campaignId);
        Task<IEnumerable<AbTest>> GetByStatusAsync(string status);
        Task<IEnumerable<AbTest>> GetActiveTestsAsync();
        Task<AbTest?> GetByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name);
        Task<IEnumerable<AbTest>> GetRunningTestsAsync();
        Task<IEnumerable<AbTest>> GetCompletedTestsAsync();
    }
}