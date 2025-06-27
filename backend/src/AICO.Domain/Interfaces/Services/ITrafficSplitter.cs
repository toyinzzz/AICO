using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface for traffic splitting operations in A/B tests
    /// </summary>
    public interface ITrafficSplitter
    {
        /// <summary>
        /// Assigns a variant based on traffic splitting percentages
        /// </summary>
        /// <param name="variants">List of variants with traffic allocations</param>
        /// <param name="userId">User ID for consistent assignment (optional)</param>
        /// <returns>Selected variant</returns>
        Variant? AssignVariant(IEnumerable<Variant> variants, string? userId = null);

        /// <summary>
        /// Validates that traffic allocations sum to 100%
        /// </summary>
        /// <param name="variants">List of variants to validate</param>
        /// <returns>True if allocations are valid</returns>
        bool ValidateTrafficAllocations(IEnumerable<Variant> variants);

        /// <summary>
        /// Calculates even traffic split percentages for variants
        /// </summary>
        /// <param name="variantCount">Number of variants</param>
        /// <returns>Traffic percentage per variant</returns>
        decimal CalculateEvenSplit(int variantCount);
    }
}