using AICO.Domain.Entities;

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