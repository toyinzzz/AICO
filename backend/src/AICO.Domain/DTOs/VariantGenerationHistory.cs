namespace AICO.Domain.DTOs
{
    /// <summary>
    /// History record of variant generation
    /// </summary>
    public class VariantGenerationHistory
    {
        /// <summary>
        /// Unique identifier for the generation session
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Campaign ID this generation belongs to
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// User who initiated the generation
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Original page URL that was analyzed
        /// </summary>
        public string OriginalPageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Generation request parameters
        /// </summary>
        public VariantGenerationRequest Request { get; set; } = new();

        /// <summary>
        /// Number of variants generated
        /// </summary>
        public int VariantsGenerated { get; set; }

        /// <summary>
        /// Generation status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Error message if generation failed
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Time taken for generation (in milliseconds)
        /// </summary>
        public long GenerationTimeMs { get; set; }

        /// <summary>
        /// AI model used for generation
        /// </summary>
        public string ModelUsed { get; set; } = string.Empty;

        /// <summary>
        /// Generation timestamp
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Quality metrics of generated variants
        /// </summary>
        public Dictionary<string, object> QualityMetrics { get; set; } = new();

        /// <summary>
        /// Additional metadata
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}