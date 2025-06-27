using AICO.Domain.Entities;
using AICO.Domain.Interfaces.ExternalServices;
using AICO.Domain.DTOs;

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for generating A/B test variants using AI
    /// </summary>
    public class AIVariantGenerator
    {
        private readonly IAIService _aiService;

        public AIVariantGenerator(IAIService aiService)
        {
            _aiService = aiService ?? throw new ArgumentNullException(nameof(aiService));
        }

        /// <summary>
        /// Generates multiple variants for the given content
        /// </summary>
        /// <param name="originalContent">Original content to create variants from</param>
        /// <param name="numberOfVariants">Number of variants to generate</param>
        /// <param name="abTestId">ID of the A/B test</param>
        /// <returns>List of generated variants</returns>
        public async Task<List<Variant>> GenerateVariants(string originalContent, int numberOfVariants, Guid abTestId)
        {
            if (string.IsNullOrWhiteSpace(originalContent))
            {
                throw new ArgumentException("Original content cannot be null or empty", nameof(originalContent));
            }

            if (numberOfVariants <= 0)
            {
                throw new ArgumentException("Number of variants must be greater than 0", nameof(numberOfVariants));
            }

            var generatedContents = await _aiService.GenerateContentVariationsAsync(originalContent, numberOfVariants);
            var variants = new List<Variant>();

            for (int i = 0; i < generatedContents.Count; i++)
            {
                var variant = new Variant(
                    name: $"AI Generated Variant {i + 1}",
                    abTestId: abTestId,
                    content: generatedContents[i],
                    trafficAllocation: 0, // Will be set later
                    isControl: false
                );
                variants.Add(variant);
            }

            return variants;
        }

        /// <summary>
        /// Generates headline variations using AI
        /// </summary>
        /// <param name="originalHeadline">Original headline</param>
        /// <param name="targetAudience">Target audience description</param>
        /// <param name="count">Number of variations to generate</param>
        /// <returns>List of headline variations</returns>
        public async Task<List<string>> GenerateHeadlineVariations(string originalHeadline, string targetAudience, int count = 5)
        {
            var prompt = $"Generate headline variations for: {originalHeadline}. Target audience: {targetAudience}";
            return await _aiService.GenerateHeadlinesAsync(prompt, count);
        }

        /// <summary>
        /// Generates CTA (Call-to-Action) variations using AI
        /// </summary>
        /// <param name="originalCta">Original CTA text</param>
        /// <param name="conversionGoal">Conversion goal description</param>
        /// <param name="count">Number of variations to generate</param>
        /// <returns>List of CTA variations</returns>
        public async Task<List<string>> GenerateCtaVariations(string originalCta, string conversionGoal, int count = 5)
        {
            var prompt = $"Generate CTA variations for: {originalCta}. Conversion goal: {conversionGoal}";
            return await _aiService.GenerateCtasAsync(prompt, count);
        }

        /// <summary>
        /// Optimizes content for a specific target audience
        /// </summary>
        /// <param name="content">Content to optimize</param>
        /// <param name="targetAudience">Target audience description</param>
        /// <returns>Optimized content</returns>
        public async Task<string> OptimizeContent(string content, string targetAudience)
        {
            return await _aiService.OptimizeContentAsync(content, targetAudience);
        }

        /// <summary>
        /// Analyzes page content and provides performance insights
        /// </summary>
        /// <param name="pageUrl">URL of the page to analyze</param>
        /// <returns>Analysis results</returns>
        public async Task<string> AnalyzePage(string pageUrl)
        {
            return await _aiService.AnalyzePageAsync(pageUrl);
        }
    }
}