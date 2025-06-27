using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for handling traffic splitting logic for A/B test variants
    /// </summary>
    public class TrafficSplitter : ITrafficSplitter
    {
        /// <summary>
        /// Assigns a variant based on traffic splitting percentages
        /// </summary>
        /// <param name="variants">List of variants with traffic allocations</param>
        /// <param name="userId">User ID for consistent assignment (optional)</param>
        /// <returns>Selected variant</returns>
        public Variant? AssignVariant(IEnumerable<Variant> variants, string? userId = null)
        {
            if (variants == null || !variants.Any())
            {
                return null;
            }

            var variantList = variants.ToList();
            var totalTrafficAllocation = variantList.Sum(v => v.TrafficAllocation);

            if (totalTrafficAllocation <= 0)
            {
                // Fallback to random selection if traffic allocations are not set correctly
                var random = new Random();
                return variantList[random.Next(variantList.Count)];
            }

            // Use user ID for consistent assignment if provided, otherwise use random
            double randomValue;
            if (!string.IsNullOrEmpty(userId))
            {
                // Use hash of user ID for consistent assignment
                var hash = userId.GetHashCode();
                randomValue = Math.Abs(hash % 10000) / 10000.0;
            }
            else
            {
                randomValue = new Random().NextDouble();
            }

            var randomNumber = randomValue * (double)totalTrafficAllocation;
            decimal cumulative = 0;

            foreach (var variant in variantList)
            {
                cumulative += variant.TrafficAllocation;
                if (randomNumber <= (double)cumulative)
                {
                    return variant;
                }
            }

            // Fallback to last variant
            return variantList.Last();
        }

        /// <summary>
        /// Validates that traffic allocations sum to 100%
        /// </summary>
        /// <param name="variants">List of variants to validate</param>
        /// <returns>True if allocations are valid</returns>
        public bool ValidateTrafficAllocations(IEnumerable<Variant> variants)
        {
            if (variants == null || !variants.Any())
            {
                return false;
            }

            var totalAllocation = variants.Sum(v => v.TrafficAllocation);
            return Math.Abs(totalAllocation - 100) < 0.01m; // Allow for small floating point differences
        }

        /// <summary>
        /// Distributes traffic evenly among variants
        /// </summary>
        /// <param name="variantCount">Number of variants</param>
        /// <returns>Traffic allocation percentage per variant</returns>
        public decimal CalculateEvenSplit(int variantCount)
        {
            if (variantCount <= 0)
            {
                return 0;
            }

            return 100m / variantCount;
        }
    }
}