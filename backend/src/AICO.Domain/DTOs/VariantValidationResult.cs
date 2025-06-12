namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Result of variant validation
    /// </summary>
    public class VariantValidationResult
    {
        /// <summary>
        /// Whether the variant is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Validation errors if any
        /// </summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>
        /// Validation warnings
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Quality score (0-100)
        /// </summary>
        public int QualityScore { get; set; }

        /// <summary>
        /// Accessibility compliance score
        /// </summary>
        public int AccessibilityScore { get; set; }

        /// <summary>
        /// Performance impact assessment
        /// </summary>
        public string PerformanceImpact { get; set; } = string.Empty;

        /// <summary>
        /// Brand compliance check
        /// </summary>
        public bool BrandCompliant { get; set; }

        /// <summary>
        /// Conversion potential score
        /// </summary>
        public double ConversionPotential { get; set; }

        /// <summary>
        /// Validation timestamp
        /// </summary>
        public DateTime ValidatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Additional validation notes
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }
}