using System.ComponentModel.DataAnnotations;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a variant in an A/B test
    /// </summary>
    public class Variant : BaseEntity
    {
        /// <summary>
        /// Variant name
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; private set; }

        /// <summary>
        /// A/B test this variant belongs to
        /// </summary>
        [Required]
        public Guid AbTestId { get; private set; }
        public AbTest? AbTest { get; private set; }

        /// <summary>
        /// Variant content (HTML/text)
        /// </summary>
        [Required]
        public string Content { get; private set; }

        /// <summary>
        /// Traffic allocation percentage for this variant
        /// </summary>
        [Range(0, 100)]
        public int TrafficAllocation { get; private set; }

        /// <summary>
        /// Whether this is the control variant
        /// </summary>
        public bool IsControl { get; private set; }

        /// <summary>
        /// AI confidence score for this variant (0-100)
        /// </summary>
        [Range(0, 100)]
        public int? AiConfidenceScore { get; private set; }

        /// <summary>
        /// AI generation prompt used
        /// </summary>
        [MaxLength(2000)]
        public string? AiPrompt { get; private set; }

        /// <summary>
        /// Number of views for this variant
        /// </summary>
        public int Views { get; private set; }

        /// <summary>
        /// Number of conversions for this variant
        /// </summary>
        public int Conversions { get; private set; }

        /// <summary>
        /// Private constructor for EF Core
        /// </summary>
        private Variant() { }

        /// <summary>
        /// Creates a new variant
        /// </summary>
        public Variant(string name, Guid abTestId, string content, int trafficAllocation,
                      bool isControl = false, string? aiPrompt = null, int? aiConfidenceScore = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AbTestId = abTestId;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            TrafficAllocation = trafficAllocation;
            IsControl = isControl;
            AiPrompt = aiPrompt;
            AiConfidenceScore = aiConfidenceScore;
        }

        /// <summary>
        /// Records a view for this variant
        /// </summary>
        public void RecordView()
        {
            Views++;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Records a conversion for this variant
        /// </summary>
        public void RecordConversion()
        {
            Conversions++;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the conversion rate for this variant
        /// </summary>
        public double GetConversionRate()
        {
            return Views > 0 ? (double)Conversions / Views * 100 : 0;
        }
    }
}