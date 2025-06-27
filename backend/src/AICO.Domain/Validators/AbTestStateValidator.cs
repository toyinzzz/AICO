using AICO.Domain.Entities;

namespace AICO.Domain.Validators
{
    /// <summary>
    /// Validator for A/B test state transitions
    /// </summary>
    public static class AbTestStateValidator
    {
        /// <summary>
        /// Validates if a state transition is allowed
        /// </summary>
        /// <param name="currentStatus">Current status of the A/B test</param>
        /// <param name="newStatus">New status to transition to</param>
        /// <exception cref="InvalidOperationException">Thrown when transition is not allowed</exception>
        public static void ValidateTransition(AbTestStatus currentStatus, AbTestStatus newStatus)
        {
            var isValidTransition = (currentStatus, newStatus) switch
            {
                // From Draft
                (AbTestStatus.Draft, AbTestStatus.Running) => true,
                (AbTestStatus.Draft, AbTestStatus.Stopped) => true,
                
                // From Running
                (AbTestStatus.Running, AbTestStatus.Stopped) => true,
                (AbTestStatus.Running, AbTestStatus.Completed) => true,
                
                // From Stopped
                (AbTestStatus.Stopped, AbTestStatus.Running) => true,
                (AbTestStatus.Stopped, AbTestStatus.Completed) => true,
                
                // Same status (no change)
                _ when currentStatus == newStatus => true,
                
                // All other transitions are invalid
                _ => false
            };

            if (!isValidTransition)
            {
                throw new InvalidOperationException(
                    $"Invalid state transition from {currentStatus} to {newStatus}");
            }
        }
    }
}