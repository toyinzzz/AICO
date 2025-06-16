using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for validating A/B test state transitions
    /// </summary>
    public interface IAbTestStateValidationService
    {
        /// <summary>
        /// Checks if a state transition is valid
        /// </summary>
        bool CanTransitionTo(AbTestStatus from, AbTestStatus to);

        /// <summary>
        /// Validates a state transition and throws if invalid
        /// </summary>
        void ValidateTransition(AbTestStatus from, AbTestStatus to);

        /// <summary>
        /// Gets all valid transitions from a given state
        /// </summary>
        IEnumerable<AbTestStatus> GetValidTransitions(AbTestStatus from);
    }
}