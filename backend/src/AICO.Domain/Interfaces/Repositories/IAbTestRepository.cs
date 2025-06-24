using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    public interface IAbTestRepository : IRepository<AbTest>
    {
        Task<IEnumerable<AbTest>> GetByCampaignIdAsync(Guid campaignId);
        Task<IEnumerable<AbTest>> GetByStatusAsync(AbTestStatus status); // Changed string to AbTestStatus
        Task<IEnumerable<AbTest>> GetActiveAsync(); // Renamed from GetActiveTestsAsync
        Task<AbTest?> GetByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name);
        // GetRunningTestsAsync and GetCompletedTestsAsync can be covered by GetByStatusAsync
        // Task<IEnumerable<AbTest>> GetRunningTestsAsync(); 
        // Task<IEnumerable<AbTest>> GetCompletedTestsAsync();
    }
}