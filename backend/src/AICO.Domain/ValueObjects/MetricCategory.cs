namespace AICO.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a metric category
    /// </summary>
    public sealed record MetricCategory
    {
        /// <summary>
        /// Common metric categories
        /// </summary>
        public static class Common
        {
            public static readonly MetricCategory Performance = new("performance");
            public static readonly MetricCategory Conversion = new("conversion");
            public static readonly MetricCategory Engagement = new("engagement");
            public static readonly MetricCategory SEO = new("seo");
            public static readonly MetricCategory UX = new("ux");
            public static readonly MetricCategory Custom = new("custom");
        }

        /// <summary>
        /// The metric category value
        /// </summary>
        public string Value { get; }

        private MetricCategory(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new MetricCategory value object
        /// </summary>
        /// <param name="value">Raw metric category string</param>
        /// <returns>MetricCategory value object</returns>
        /// <exception cref="ArgumentException">Thrown when metric category is invalid</exception>
        public static MetricCategory Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Metric category cannot be empty", nameof(value));

            return new MetricCategory(value.Trim().ToLowerInvariant());
        }

        public override string ToString() => Value;
    }
}