using System;
using System.Collections.Generic;
using System.Linq;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for validating campaign state transitions
    /// </summary>
    public class CampaignStateValidationService : ICampaignStateValidationService
    {
        private static readonly Dictionary<CampaignStatus, List<CampaignStatus>> ValidTransitions = new()
        {
            { CampaignStatus.Draft, new List<CampaignStatus> { CampaignStatus.Active } },
            { CampaignStatus.Active, new List<CampaignStatus> { CampaignStatus.Stopped, CampaignStatus.Completed } },
            { CampaignStatus.Stopped, new List<CampaignStatus> { CampaignStatus.Active, CampaignStatus.Completed } },
            { CampaignStatus.Completed, new List<CampaignStatus>() } // No transitions from completed
        };

        /// <summary>
        /// Checks if a state transition is valid
        /// </summary>
        public bool CanTransitionTo(CampaignStatus from, CampaignStatus to)
        {
            return ValidTransitions.ContainsKey(from) && ValidTransitions[from].Contains(to);
        }

        /// <summary>
        /// Validates a state transition and throws if invalid
        /// </summary>
        public void ValidateTransition(CampaignStatus from, CampaignStatus to)
        {
            if (!CanTransitionTo(from, to))
            {
                throw new InvalidOperationException(
                    $"Invalid campaign state transition from {from} to {to}. " +
                    $"Valid transitions from {from} are: {string.Join(", ", GetValidTransitions(from))}");
            }
        }

        /// <summary>
        /// Gets all valid transitions from a given state
        /// </summary>
        public IEnumerable<CampaignStatus> GetValidTransitions(CampaignStatus from)
        {
            return ValidTransitions.ContainsKey(from) 
                ? ValidTransitions[from].AsEnumerable() 
                : Enumerable.Empty<CampaignStatus>();
        }
    }
}