using AICO.Domain.Events;
using AICO.Domain.ValueObjects;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a conversion event on a website
    /// </summary>
    public class Conversion : BaseEntity
    {
        /// <summary>
        /// The ID of the website where the conversion occurred
        /// </summary>
        public Guid WebsiteId { get; private set; }

        /// <summary>
        /// The ID of the session in which the conversion occurred
        /// </summary>
        public Guid? SessionId { get; private set; }

        /// <summary>
        /// The type of conversion
        /// </summary>
        public ConversionType ConversionType { get; private set; }

        /// <summary>
        /// The name of the conversion goal
        /// </summary>
        public string GoalName { get; private set; }

        /// <summary>
        /// The monetary value of the conversion (if applicable)
        /// </summary>
        public decimal? Value { get; private set; }

        /// <summary>
        /// The currency of the value (if applicable)
        /// </summary>
        public string? Currency { get; private set; }

        /// <summary>
        /// Additional data about the conversion in JSON format
        /// </summary>
        public string? ConversionData { get; private set; }

        /// <summary>
        /// When the conversion occurred
        /// </summary>
        public DateTime ConvertedAt { get; private set; }

        /// <summary>
        /// Navigation property to the website
        /// </summary>
        public virtual Website? Website { get; private set; }

        /// <summary>
        /// Navigation property to the session
        /// </summary>
        public virtual Session? Session { get; private set; }

        // Private constructor for EF Core
        private Conversion() { }

        /// <summary>
        /// Creates a new conversion
        /// </summary>
        public static Conversion Create(
            Guid websiteId,
            ConversionType conversionType,
            string? goalName, // Made nullable
            decimal? value = null,
            string? currency = null,
            string? conversionData = null,
            Guid? sessionId = null)
        {
            if (conversionType == null)
                throw new ArgumentNullException(nameof(conversionType));

            if (string.IsNullOrWhiteSpace(goalName))
                throw new ArgumentException("Goal name cannot be null or empty", nameof(goalName));

            if (value.HasValue && value.Value < 0)
                throw new ArgumentException("Value cannot be negative", nameof(value));

            if (value.HasValue && string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency must be provided when value is specified", nameof(currency));

            var conversion = new Conversion
            {
                WebsiteId = websiteId,
                SessionId = sessionId,
                ConversionType = conversionType,
                GoalName = goalName,
                Value = value,
                Currency = currency,
                ConversionData = conversionData,
                ConvertedAt = DateTime.UtcNow
            };

            conversion.AddDomainEvent(new ConversionRecorded(
                conversion.Id,
                websiteId,
                conversionType,
                goalName,
                value,
                sessionId));

            return conversion;
        }

        /// <summary>
        /// Creates a new conversion
        /// </summary>
        public static Conversion Create(
            Guid websiteId,
            string? conversionType, // Made nullable
            string? goalName, // Made nullable
            decimal? value = null,
            string? currency = null,
            string? conversionData = null,
            Guid? sessionId = null)
        {
            if (string.IsNullOrWhiteSpace(conversionType))
                throw new ArgumentException("Conversion type cannot be null or empty", nameof(conversionType));

            return Create(
                websiteId,
                ConversionType.Create(conversionType), 
                goalName,
                value,
                currency,
                conversionData,
                sessionId);
        }
    }
}