using AICO.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Commands
{
    // Command Records
    public record TrackConversionCommand(
        Guid WebsiteId,
        Guid CampaignId, // Optional, can be null if not tied to a specific campaign
        Guid? AbTestId,    // Optional, can be null
        Guid? VariantId,   // Optional, can be null
        string EventName,
        string UserId, // Or SessionId, depending on tracking strategy
        DateTime Timestamp,
        decimal? Value,    // Optional, for revenue tracking
        string? Url,       // Optional, page where conversion happened
        string? Referrer,  // Optional
        string? UserAgent  // Optional
    ) : ICommand;

    // Command Handler Interface
    public interface IConversionEventCommandHandler :
        ICommandHandler<TrackConversionCommand>
    {
    }
}