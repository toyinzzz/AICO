using AICO.Domain.Events;
using AICO.Domain.ValueObjects;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a performance or business metric for a website
    /// </summary>
    public class Metric : BaseEntity
    {
        /// <summary>
        /// The ID of the website this metric belongs to
        /// </summary>
        public Guid WebsiteId { get; private set; }

        /// <summary>
        /// The name of the metric
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The category of the metric
        /// </summary>
        public MetricCategory Category { get; private set; }

        /// <summary>
        /// The value of the metric
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// The unit of measurement (e.g., seconds, percentage, count)
        /// </summary>
        public string? Unit { get; private set; }

        /// <summary>
        /// The date this metric was recorded
        /// </summary>
        public DateTime RecordedAt { get; private set; }

        /// <summary>
        /// Optional dimension for the metric (e.g., device type, browser)
        /// </summary>
        public string? Dimension { get; private set; }

        /// <summary>
        /// Optional dimension value
        /// </summary>
        public string? DimensionValue { get; private set; }

        /// <summary>
        /// Navigation property to the website
        /// </summary>
        public virtual Website? Website { get; private set; }

        // Private constructor for EF Core
        private Metric() { }

        /// <summary>
        /// Creates a new metric
        /// </summary>
        public static Metric Create(
            Guid websiteId,
            string name,
            MetricCategory category,
            double value,
            string? unit = null,
            string? dimension = null,
            string? dimensionValue = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Metric name cannot be null or empty", nameof(name));

            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var metric = new Metric
            {
                WebsiteId = websiteId,
                Name = name,
                Category = category,
                Value = value,
                Unit = unit,
                Dimension = dimension,
                DimensionValue = dimensionValue,
                RecordedAt = DateTime.UtcNow
            };

            metric.AddDomainEvent(new MetricRecorded(metric.Id, websiteId, name, category, value));

            return metric;
        }

        /// <summary>
        /// Creates a new metric
        /// </summary>
        public static Metric Create(
            Guid websiteId,
            string name,
            string category,
            double value,
            string? unit = null,
            string? dimension = null,
            string? dimensionValue = null)
        {
            return Create(
                websiteId,
                name,
                MetricCategory.Create(category),
                value,
                unit,
                dimension,
                dimensionValue);
        }

        /// <summary>
        /// Updates the metric value
        /// </summary>
        public void UpdateValue(double newValue)
        {
            Value = newValue;
            RecordedAt = DateTime.UtcNow;
            MarkAsUpdated();

            AddDomainEvent(new MetricRecorded(Id, WebsiteId, Name, Category, newValue));
        }
    }
}