namespace AICO.Domain.DTOs
{
    /// <summary>
    /// DTO for updating snippet configuration
    /// </summary>
    public class SnippetConfigDto
    {
        /// <summary>
        /// Whether snippet is enabled
        /// </summary>
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// Custom tracking events to capture
        /// </summary>
        public List<string>? TrackingEvents { get; set; }

        /// <summary>
        /// Custom CSS selectors for tracking
        /// </summary>
        public Dictionary<string, string>? CustomSelectors { get; set; }

        /// <summary>
        /// Snippet loading strategy (async, defer, blocking)
        /// </summary>
        public string? LoadingStrategy { get; set; }

        /// <summary>
        /// Whether to track page views automatically
        /// </summary>
        public bool? AutoTrackPageViews { get; set; }

        /// <summary>
        /// Whether to track clicks automatically
        /// </summary>
        public bool? AutoTrackClicks { get; set; }

        /// <summary>
        /// Whether to track form submissions
        /// </summary>
        public bool? AutoTrackForms { get; set; }

        /// <summary>
        /// Custom configuration options
        /// </summary>
        public Dictionary<string, object>? CustomOptions { get; set; }
    }
}