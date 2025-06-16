using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    public interface ISnippetRepository : IRepository<Snippet>
    {
        Task<IEnumerable<Snippet>> GetByWebsiteIdAsync(Guid websiteId);
        Task<IEnumerable<Snippet>> GetByCampaignIdAsync(Guid campaignId);
        Task<Snippet?> GetActiveSnippetAsync(Guid websiteId);
        Task<IEnumerable<Snippet>> GetByStatusAsync(string status);
        Task<Snippet?> GetByCodeAsync(string code);
        Task<bool> ExistsByCodeAsync(string code);
    }
}