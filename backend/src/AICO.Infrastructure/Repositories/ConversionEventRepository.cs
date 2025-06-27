using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories;

public class ConversionEventRepository : BaseRepository<ConversionEvent>, IConversionEventRepository
{
    public ConversionEventRepository(AicoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ConversionEvent>> GetByCampaignIdAsync(Guid campaignId, DateTime startDate, DateTime endDate)
    {
        return await _context.ConversionEvents
            .Where(ce => ce.CampaignId == campaignId && 
                        ce.EventTimestamp >= startDate && 
                        ce.EventTimestamp <= endDate)
            .OrderByDescending(ce => ce.EventTimestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<ConversionEvent>> GetByAbTestIdAsync(Guid abTestId, DateTime startDate, DateTime endDate)
    {
        return await _context.ConversionEvents
            .Where(ce => ce.AbTestId == abTestId && 
                        ce.EventTimestamp >= startDate && 
                        ce.EventTimestamp <= endDate)
            .OrderByDescending(ce => ce.EventTimestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<ConversionEvent>> GetByVariantIdAsync(Guid variantId, DateTime startDate, DateTime endDate)
    {
        return await _context.ConversionEvents
            .Where(ce => ce.VariantId == variantId && 
                        ce.EventTimestamp >= startDate && 
                        ce.EventTimestamp <= endDate)
            .OrderByDescending(ce => ce.EventTimestamp)
            .ToListAsync();
    }
}
