using System.ComponentModel.DataAnnotations;
using AICO.Domain.Entities;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents revenue/profit tracking for conversions
    /// </summary>
    public class Revenue : BaseEntity
    {
        /// <summary>
        /// Website this revenue belongs to
        /// </summary>
        [Required]
        public Guid WebsiteId { get; private set; }
        public Website Website { get; private set; }

        /// <summary>
        /// User who owns this revenue record
        /// </summary>
        [Required]
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        /// <summary>
        /// Associated conversion (optional)
        /// </summary>
        public Guid? ConversionId { get; private set; }
        public Conversion Conversion { get; private set; }

        /// <summary>
        /// Associated campaign (optional)
        /// </summary>
        public Guid? CampaignId { get; private set; }
        public Campaign Campaign { get; private set; }

        /// <summary>
        /// Revenue amount
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; private set; }

        /// <summary>
        /// Currency code (ISO 4217)
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string Currency { get; private set; } = "USD";

        /// <summary>
        /// Revenue source (e.g., "Purchase", "Subscription", "Lead")
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Source { get; private set; }

        /// <summary>
        /// Transaction ID or reference
        /// </summary>
        [MaxLength(200)]
        public string TransactionId { get; private set; }

        /// <summary>
        /// Additional metadata as JSON
        /// </summary>
        public string Metadata { get; private set; }

        /// <summary>
        /// Date when revenue was generated
        /// </summary>
        [Required]
        public DateTime RevenueDate { get; private set; }

        /// <summary>
        /// Session ID for tracking
        /// </summary>
        public Guid? SessionId { get; private set; }

        /// <summary>
        /// Variant ID for A/B testing
        /// </summary>
        public Guid? VariantId { get; private set; }

        /// <summary>
        /// Private constructor for EF Core
        /// </summary>
        private Revenue() { }

        /// <summary>
        /// Creates a new revenue record
        /// </summary>
        public Revenue(Guid websiteId, Guid userId, decimal amount, string source,
                      DateTime? revenueDate = null, string currency = "USD",
                      Guid? conversionId = null, Guid? campaignId = null,
                      string transactionId = null, string metadata = null,
                      Guid? sessionId = null, Guid? variantId = null)
        {
            WebsiteId = websiteId;
            UserId = userId;
            Amount = amount;
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Currency = currency ?? "USD";
            ConversionId = conversionId;
            CampaignId = campaignId;
            TransactionId = transactionId;
            Metadata = metadata;
            RevenueDate = revenueDate ?? DateTime.UtcNow;
            SessionId = sessionId;
            VariantId = variantId;
        }

        /// <summary>
        /// Updates the revenue amount
        /// </summary>
        public void UpdateAmount(decimal newAmount)
        {
            if (newAmount < 0)
                throw new ArgumentException("Revenue amount cannot be negative", nameof(newAmount));

            Amount = newAmount;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

public class Revenue : BaseEntity
{
    public Guid SessionId { get; set; }
    public Guid VariantId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime RecordedAt { get; set; }
    
    // Navigation properties
    public Session Session { get; set; }
    public Variant Variant { get; set; }
}