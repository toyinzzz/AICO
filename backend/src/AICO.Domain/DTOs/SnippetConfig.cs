namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Configuration for JavaScript snippet
    /// </summary>
    public class SnippetConfig
    {
        /// <summary>
        /// Website ID this configuration belongs to
        /// </summary>
        public Guid WebsiteId { get; set; }

        /// <summary>
        /// Whether snippet is enabled
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Snippet version
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// Custom tracking events to capture
        /// </summary>
        public List<string> TrackingEvents { get; set; } = new();

        /// <summary>
        /// Custom CSS selectors for tracking
        /// </summary>
        public Dictionary<string, string> CustomSelectors { get; set; } = new();

        /// <summary>
        /// Snippet loading strategy (async, defer, blocking)
        /// </summary>
        public string LoadingStrategy { get; set; } = "async";

        /// <summary>
        /// Whether to track page views automatically
        /// </summary>
        public bool AutoTrackPageViews { get; set; } = true;

        /// <summary>
        /// Whether to track clicks automatically
        /// </summary>
        public bool AutoTrackClicks { get; set; } = true;

        /// <summary>
        /// Whether to track form submissions
        /// </summary>
        public bool AutoTrackForms { get; set; } = true;

        /// <summary>
        /// Custom configuration options
        /// </summary>
        public Dictionary<string, object> CustomOptions { get; set; } = new();

        /// <summary>
        /// Configuration created timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Configuration last updated timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}