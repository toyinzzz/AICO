using AICO.Domain.Entities;

namespace AICO.Domain.Validators
{
    /// <summary>
    /// Validator for campaign state transitions
    /// </summary>
    public static class CampaignStateValidator
    {
        /// <summary>
        /// Validates if a state transition is allowed
        /// </summary>
        /// <param name="currentStatus">Current status of the campaign</param>
        /// <param name="newStatus">New status to transition to</param>
        /// <exception cref="InvalidOperationException">Thrown when transition is not allowed</exception>
        public static void ValidateTransition(CampaignStatus currentStatus, CampaignStatus newStatus)
        {
            var isValidTransition = (currentStatus, newStatus) switch
            {
                // From Draft
                (CampaignStatus.Draft, CampaignStatus.Active) => true,
                (CampaignStatus.Draft, CampaignStatus.Stopped) => true,
                
                // From Active
                (CampaignStatus.Active, CampaignStatus.Stopped) => true,
                (CampaignStatus.Active, CampaignStatus.Completed) => true,
                
                // From Stopped
                (CampaignStatus.Stopped, CampaignStatus.Active) => true,
                (CampaignStatus.Stopped, CampaignStatus.Completed) => true,
                
                // Same status (no change)
                _ when currentStatus == newStatus => true,
                
                // All other transitions are invalid
                _ => false
            };

            if (!isValidTransition)
            {
                throw new InvalidOperationException(
                    $"Invalid campaign state transition from {currentStatus} to {newStatus}");
            }
        }
    }
}