namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Analysis result of a web page
    /// </summary>
    public class PageAnalysis
    {
        /// <summary>
        /// URL of the analyzed page
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Page title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Main headline
        /// </summary>
        public string MainHeadline { get; set; } = string.Empty;

        /// <summary>
        /// Primary call-to-action text
        /// </summary>
        public string PrimaryCta { get; set; } = string.Empty;

        /// <summary>
        /// Key content sections
        /// </summary>
        public List<string> ContentSections { get; set; } = new();

        /// <summary>
        /// Identified conversion elements
        /// </summary>
        public List<string> ConversionElements { get; set; } = new();

        /// <summary>
        /// Page structure analysis
        /// </summary>
        public string PageStructure { get; set; } = string.Empty;

        /// <summary>
        /// Detected industry/business type
        /// </summary>
        public string DetectedIndustry { get; set; } = string.Empty;

        /// <summary>
        /// Target audience insights
        /// </summary>
        public string TargetAudienceInsights { get; set; } = string.Empty;

        /// <summary>
        /// Performance optimization suggestions
        /// </summary>
        public List<string> OptimizationSuggestions { get; set; } = new();

        /// <summary>
        /// Analysis timestamp
        /// </summary>
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Analysis confidence score (0-1)
        /// </summary>
        public double ConfidenceScore { get; set; }
    }
}