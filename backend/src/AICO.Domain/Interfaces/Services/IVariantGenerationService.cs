using AICO.Domain.DTOs;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service interface for AI-powered variant generation
    /// </summary>
    public interface IVariantGenerationService
    {
        /// <summary>
        /// Generates multiple variants for a landing page
        /// </summary>
        Task<List<Variant>> GenerateVariantsAsync(string originalPageUrl, VariantGenerationRequest request);

        /// <summary>
        /// Creates a custom variant manually
        /// </summary>
        Task<Variant> CreateCustomVariantAsync(Guid campaignId, string name, string htmlContent, string description = null);

        /// <summary>
        /// Analyzes page content and structure
        /// </summary>
        Task<PageAnalysis> AnalyzePageContentAsync(string pageUrl);

        /// <summary>
        /// Generates copy variations for specific elements
        /// </summary>
        Task<List<string>> GenerateCopyVariationsAsync(string originalCopy, string targetAudience, string goal);

        /// <summary>
        /// Generates headline variations
        /// </summary>
        Task<List<string>> GenerateHeadlineVariationsAsync(string originalHeadline, string targetAudience, int count = 5);

        /// <summary>
        /// Generates CTA button variations
        /// </summary>
        Task<List<string>> GenerateCtaVariationsAsync(string originalCta, string conversionGoal, int count = 5);

        /// <summary>
        /// Validates generated variant content
        /// </summary>
        Task<VariantValidationResult> ValidateVariantAsync(Variant variant);

        /// <summary>
        /// Gets variant generation history for a campaign
        /// </summary>
        Task<IEnumerable<VariantGenerationHistory>> GetGenerationHistoryAsync(Guid campaignId);

        /// <summary>
        /// Estimates variant performance before testing
        /// </summary>
        Task<VariantPerformancePrediction> PredictVariantPerformanceAsync(Variant variant, PageAnalysis baselineAnalysis);
    }
}