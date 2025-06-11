using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for validating campaign state transitions
    /// </summary>
    public interface ICampaignStateValidationService
    {
        /// <summary>
        /// Checks if a state transition is valid
        /// </summary>
        bool CanTransitionTo(CampaignStatus from, CampaignStatus to);
        
        /// <summary>
        /// Validates a state transition and throws if invalid
        /// </summary>
        void ValidateTransition(CampaignStatus from, CampaignStatus to);
        
        /// <summary>
        /// Gets all valid transitions from a given state
        /// </summary>
        IEnumerable<CampaignStatus> GetValidTransitions(CampaignStatus from);
    }
}