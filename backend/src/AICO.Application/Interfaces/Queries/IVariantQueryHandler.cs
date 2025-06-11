using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Queries
{
    public interface IVariantQueryHandler
    {
        Task<Variant?> GetVariantByIdAsync(Guid id);
        Task<IEnumerable<Variant>> GetVariantsByAbTestIdAsync(Guid abTestId);
        Task<IEnumerable<Variant>> GetVariantsByCampaignIdAsync(Guid campaignId);
        Task<Variant?> GetControlVariantAsync(Guid abTestId);
        Task<IEnumerable<Variant>> GetTestVariantsAsync(Guid abTestId);
    }
}