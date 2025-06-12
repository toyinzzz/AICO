using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    public interface IVariantRepository : IRepository<Variant>
    {
        Task<IEnumerable<Variant>> GetByAbTestIdAsync(Guid abTestId);
        Task<IEnumerable<Variant>> GetByCampaignIdAsync(Guid campaignId);
        Task<Variant?> GetControlVariantAsync(Guid abTestId);
        Task<IEnumerable<Variant>> GetTestVariantsAsync(Guid abTestId);
        Task<Variant?> GetByNameAsync(string name, Guid abTestId);
        Task<bool> ExistsByNameAsync(string name, Guid abTestId);
    }
}