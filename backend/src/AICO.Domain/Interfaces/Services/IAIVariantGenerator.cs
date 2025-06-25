using AICO.Domain.Entities;
using AICO.Domain.DTOs;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface for AI-powered variant generation operations
    /// </summary>
    public interface IAIVariantGenerator
    {
        /// <summary>
        /// Generates variants for an A/B test using AI
        /// </summary>
        /// <param name="originalContent">Original content to create variants from</param>
        /// <param name="variantCount">Number of variants to generate</param>
        /// <param name="testGoal">Goal of the A/B test</param>
        /// <returns>List of generated variants</returns>
        Task<List<Variant>> GenerateVariantsAsync(string originalContent, int variantCount, string testGoal);

        /// <summary>
        /// Generates headline variants using AI
        /// </summary>
        /// <param name="originalHeadline">Original headline</param>
        /// <param name="variantCount">Number of variants to generate</param>
        /// <param name="tone">Desired tone for variants</param>
        /// <returns>List of headline variants</returns>
        Task<List<string>> GenerateHeadlineVariantsAsync(string originalHeadline, int variantCount, string tone = "persuasive");

        /// <summary>
        /// Generates CTA (Call-to-Action) variants using AI
        /// </summary>
        /// <param name="originalCta">Original CTA text</param>
        /// <param name="variantCount">Number of variants to generate</param>
        /// <param name="context">Context for the CTA</param>
        /// <returns>List of CTA variants</returns>
        Task<List<string>> GenerateCtaVariantsAsync(string originalCta, int variantCount, string context);

        /// <summary>
        /// Optimizes content for better conversion using AI
        /// </summary>
        /// <param name="content">Content to optimize</param>
        /// <param name="targetAudience">Target audience description</param>
        /// <param name="conversionGoal">Conversion goal</param>
        /// <returns>Optimized content</returns>
        Task<string> OptimizeContentAsync(string content, string targetAudience, string conversionGoal);

        /// <summary>
        /// Analyzes a webpage and suggests improvements using AI
        /// </summary>
        /// <param name="url">URL to analyze</param>
        /// <param name="analysisType">Type of analysis to perform</param>
        /// <returns>Analysis result with suggestions</returns>
        Task<AnalysisResult> AnalyzePageAsync(string url, string analysisType);
    }
}