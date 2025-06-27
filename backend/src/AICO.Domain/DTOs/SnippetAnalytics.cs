namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Analytics data for snippet performance
    /// </summary>
    public class SnippetAnalytics
    {
        /// <summary>
        /// Website ID
        /// </summary>
        public Guid WebsiteId { get; set; }

        /// <summary>
        /// Total page views tracked
        /// </summary>
        public long TotalPageViews { get; set; }

        /// <summary>
        /// Total events tracked
        /// </summary>
        public long TotalEvents { get; set; }

        /// <summary>
        /// Total sessions tracked
        /// </summary>
        public long TotalSessions { get; set; }

        /// <summary>
        /// Unique visitors count
        /// </summary>
        public long UniqueVisitors { get; set; }

        /// <summary>
        /// Conversion rate percentage
        /// </summary>
        public double ConversionRate { get; set; }

        /// <summary>
        /// Last activity timestamp
        /// </summary>
        public DateTime LastActivity { get; set; }

        /// <summary>
        /// Average session duration in seconds
        /// </summary>
        public double AverageSessionDuration { get; set; }

        /// <summary>
        /// Snippet load time in milliseconds
        /// </summary>
        public double AverageLoadTime { get; set; }

        /// <summary>
        /// Error rate percentage
        /// </summary>
        public double ErrorRate { get; set; }

        /// <summary>
        /// Data collection rate percentage
        /// </summary>
        public double DataCollectionRate { get; set; }

        /// <summary>
        /// Most tracked events
        /// </summary>
        public Dictionary<string, long> TopEvents { get; set; } = new();

        /// <summary>
        /// Browser compatibility data
        /// </summary>
        public Dictionary<string, long> BrowserStats { get; set; } = new();

        /// <summary>
        /// Device type statistics
        /// </summary>
        public Dictionary<string, long> DeviceStats { get; set; } = new();

        /// <summary>
        /// Analytics period start
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// Analytics period end
        /// </summary>
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// Last updated timestamp
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}