using AICO.Application.Interfaces.Queries;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Queries
{
    public class VariantQueryHandler : IVariantQueryHandler
    {
        private readonly IVariantRepository _variantRepository;

        public VariantQueryHandler(IVariantRepository variantRepository)
        {
            _variantRepository = variantRepository ?? throw new ArgumentNullException(nameof(variantRepository));
        }

        public Task<Variant?> HandleAsync(GetVariantByIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // return _variantRepository.GetByIdAsync(query.Id);
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Variant>> HandleAsync(GetVariantsByAbTestIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // return _variantRepository.GetByAbTestIdAsync(query.AbTestId);
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Variant>> HandleAsync(GetVariantsByCampaignIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // return _variantRepository.GetByCampaignIdAsync(query.CampaignId);
            throw new NotImplementedException();
        }

        public Task<Variant?> HandleAsync(GetControlVariantQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // return _variantRepository.GetControlVariantAsync(query.AbTestId);
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Variant>> HandleAsync(GetTestVariantsQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // return _variantRepository.GetTestVariantsAsync(query.AbTestId);
            throw new NotImplementedException();
        }
    }
}