using System.ComponentModel.DataAnnotations;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a variant within an A/B test.
    /// </summary>
    public class AbTestVariant : BaseEntity
    {
        /// <summary>
        /// The ID of the A/B test this variant belongs to.
        /// </summary>
        [Required]
        public Guid AbTestId { get; private set; }
        public AbTest AbTest { get; private set; }

        /// <summary>
        /// Name of the variant (e.g., "Variation A", "Control").
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; private set; }

        /// <summary>
        /// Optional description of the variant.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; private set; }

        /// <summary>
        /// The content or changes specific to this variant (e.g., HTML, JSON configuration).
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Percentage of traffic allocated to this variant (0-100).
        /// </summary>
        [Range(0, 100)]
        public decimal TrafficSplitPercentage { get; private set; }

        /// <summary>
        /// Number of times this variant has been viewed/displayed.
        /// </summary>
        public long Views { get; private set; }

        /// <summary>
        /// Number of conversions attributed to this variant.
        /// </summary>
        public long Conversions { get; private set; }

        /// <summary>
        /// Indicates if this is the control group variant.
        /// </summary>
        public bool IsControl { get; private set; }

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private AbTestVariant() { }

        /// <summary>
        /// Creates a new A/B test variant.
        /// </summary>
        public AbTestVariant(Guid abTestId, string name, string content, decimal trafficSplitPercentage, string? description = null, bool isControl = false)
        {
            AbTestId = abTestId;
            Name = name;
            Content = content;
            TrafficSplitPercentage = trafficSplitPercentage;
            Description = description;
            IsControl = isControl;
            Views = 0;
            Conversions = 0;
        }

        public void IncrementViews()
        {
            Views++;
        }

        public void IncrementConversions()
        {
            Conversions++;
        }

        public void UpdateDetails(string name, string? description, string content, decimal trafficSplitPercentage, bool isControl)
        {
            Name = name;
            Description = description;
            Content = content;
            TrafficSplitPercentage = trafficSplitPercentage;
            IsControl = isControl;
        }

        /// <summary>
        /// Calculates the conversion rate for this variant.
        /// </summary>
        /// <returns>The conversion rate as a percentage, or 0 if views are zero.</returns>
        public decimal GetConversionRate()
        {
            if (Views == 0)
            {
                return 0;
            }
            return (decimal)Conversions / Views * 100;
        }
    }
}