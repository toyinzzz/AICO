namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Represents an AI-generated variant for A/B testing
    /// </summary>
    public class AIGeneratedVariant
    {
        /// <summary>
        /// The generated content/HTML for the variant
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Description of the changes made in this variant
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// AI confidence score for this variant's potential performance
        /// </summary>
        public decimal ConfidenceScore { get; set; }

        /// <summary>
        /// Optional variant name/identifier
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Optimization goal this variant targets
        /// </summary>
        public string? OptimizationGoal { get; set; }
    }
}