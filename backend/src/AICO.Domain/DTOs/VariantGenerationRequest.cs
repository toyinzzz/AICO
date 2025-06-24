namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Request model for variant generation
    /// </summary>
    public class VariantGenerationRequest
    {
        /// <summary>
        /// Number of variants to generate
        /// </summary>
        public int VariantCount { get; set; } = 3;

        /// <summary>
        /// Target audience description
        /// </summary>
        public string TargetAudience { get; set; } = string.Empty;

        /// <summary>
        /// Conversion goal for the variants
        /// </summary>
        public string ConversionGoal { get; set; } = string.Empty;

        /// <summary>
        /// Specific elements to focus on (headlines, cta, copy, etc.)
        /// </summary>
        public List<string> FocusElements { get; set; } = new();

        /// <summary>
        /// Brand guidelines or constraints
        /// </summary>
        public string BrandGuidelines { get; set; } = string.Empty;

        /// <summary>
        /// Industry or business type
        /// </summary>
        public string Industry { get; set; } = string.Empty;

        /// <summary>
        /// Additional context or requirements
        /// </summary>
        public string AdditionalContext { get; set; } = string.Empty;

        /// <summary>
        /// Number of variants to generate
        /// </summary>
        public int NumberOfVariants { get; set; } = 3;

        /// <summary>
        /// Generation prompt for AI
        /// </summary>
        public string GenerationPrompt { get; set; } = string.Empty;

        /// <summary>
        /// A/B Test ID this request is for
        /// </summary>
        public Guid AbTestId { get; set; }
    }
}