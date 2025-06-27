namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Installation instructions for JavaScript snippet
    /// </summary>
    public class SnippetInstructions
    {
        /// <summary>
        /// Website ID
        /// </summary>
        public Guid WebsiteId { get; set; }

        /// <summary>
        /// Generated snippet code
        /// </summary>
        public string SnippetCode { get; set; } = string.Empty;

        /// <summary>
        /// Step-by-step installation instructions
        /// </summary>
        public List<string> InstallationSteps { get; set; } = new();

        /// <summary>
        /// Platform-specific instructions (WordPress, Shopify, etc.)
        /// </summary>
        public Dictionary<string, List<string>> PlatformInstructions { get; set; } = new();

        /// <summary>
        /// Verification steps to confirm installation
        /// </summary>
        public List<string> VerificationSteps { get; set; } = new();

        /// <summary>
        /// Common troubleshooting tips
        /// </summary>
        public List<string> TroubleshootingTips { get; set; } = new();

        /// <summary>
        /// Required HTML placement (head, body, etc.)
        /// </summary>
        public string RecommendedPlacement { get; set; } = "head";

        /// <summary>
        /// Whether async loading is recommended
        /// </summary>
        public bool UseAsyncLoading { get; set; } = true;

        /// <summary>
        /// Additional configuration options
        /// </summary>
        public Dictionary<string, string> ConfigurationOptions { get; set; } = new();

        /// <summary>
        /// Support contact information
        /// </summary>
        public string SupportContact { get; set; } = string.Empty;

        /// <summary>
        /// Documentation links
        /// </summary>
        public List<string> DocumentationLinks { get; set; } = new();

        /// <summary>
        /// Instructions generated timestamp
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}