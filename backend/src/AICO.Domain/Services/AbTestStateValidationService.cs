using System;
using System.Collections.Generic;
using System.Linq;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for validating A/B test state transitions
    /// </summary>
    public class AbTestStateValidationService : IAbTestStateValidationService
    {
        private static readonly Dictionary<AbTestStatus, List<AbTestStatus>> ValidTransitions = new()
        {
            { AbTestStatus.Draft, new List<AbTestStatus> { AbTestStatus.Running } },
            { AbTestStatus.Running, new List<AbTestStatus> { AbTestStatus.Stopped, AbTestStatus.Completed } },
            { AbTestStatus.Stopped, new List<AbTestStatus> { AbTestStatus.Running, AbTestStatus.Completed } },
            { AbTestStatus.Completed, new List<AbTestStatus>() } // No transitions from completed
        };

        /// <summary>
        /// Checks if a state transition is valid
        /// </summary>
        public bool CanTransitionTo(AbTestStatus from, AbTestStatus to)
        {
            return ValidTransitions.ContainsKey(from) && ValidTransitions[from].Contains(to);
        }

        /// <summary>
        /// Validates a state transition and throws if invalid
        /// </summary>
        public void ValidateTransition(AbTestStatus from, AbTestStatus to)
        {
            if (!CanTransitionTo(from, to))
            {
                throw new InvalidOperationException(
                    $"Invalid A/B test state transition from {from} to {to}. " +
                    $"Valid transitions from {from} are: {string.Join(", ", GetValidTransitions(from))}");
            }
        }

        /// <summary>
        /// Gets all valid transitions from a given state
        /// </summary>
        public IEnumerable<AbTestStatus> GetValidTransitions(AbTestStatus from)
        {
            return ValidTransitions.ContainsKey(from) 
                ? ValidTransitions[from].AsEnumerable() 
                : Enumerable.Empty<AbTestStatus>();
        }
    }
}