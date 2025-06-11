using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Queries
{
    public interface IAbTestQueryHandler
    {
        Task<AbTest?> GetAbTestByIdAsync(Guid id);
        Task<IEnumerable<AbTest>> GetAbTestsByCampaignIdAsync(Guid campaignId);
        Task<IEnumerable<AbTest>> GetActiveAbTestsAsync();
        Task<IEnumerable<AbTest>> GetRunningAbTestsAsync();
        Task<IEnumerable<AbTest>> GetCompletedAbTestsAsync();
    }
}