using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.Interfaces.ExternalServices;
using AICO.Shared.Interfaces;
using Microsoft.Extensions.Logging;

namespace AICO.Domain.Services
{
    public class VariantGenerationService : IVariantGenerationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAbTestRepository _abTestRepository;
        private readonly IVariantRepository _variantRepository;
        private readonly IMapper<Variant, VariantDto> _mapper; // Changed to custom IMapper generic type
        private readonly IAIService _aiService;
        private readonly ILogger<VariantGenerationService> _logger;

        public VariantGenerationService(
            IUserRepository userRepository, 
            IAbTestRepository abTestRepository, 
            IVariantRepository variantRepository,
            IMapper<Variant, VariantDto> mapper, 
            IAIService aiService,
            ILogger<VariantGenerationService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _abTestRepository = abTestRepository ?? throw new ArgumentNullException(nameof(abTestRepository));
            _variantRepository = variantRepository ?? throw new ArgumentNullException(nameof(variantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _aiService = aiService ?? throw new ArgumentNullException(nameof(aiService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserAssignedVariantDto> GetUserAbTestVariantAsync(string userId, string abTestId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or whitespace.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(abTestId))
            {
                throw new ArgumentException("A/B Test ID cannot be null or whitespace.", nameof(abTestId));
            }

            // Attempt to get the user's assigned variant
            var userAssignedVariant = await _userRepository.GetUserAbTestVariantAsync(userId, Guid.Parse(abTestId));

            if (userAssignedVariant?.Variant != null)
            {
                return userAssignedVariant;
            }

            // If no variant is assigned, or if the DTO or Variant is null, proceed to assign one.
            var abTest = await _abTestRepository.GetByIdAsync(Guid.Parse(abTestId));
            if (abTest == null || abTest.Variants.Count == 0)
            {
                _logger.LogWarning("A/B test with ID {AbTestId} not found or has no variants.", abTestId);
                return new UserAssignedVariantDto { Variant = null };
            }

            // Select a variant based on traffic splitting.
            var selectedVariant = SelectVariantByTrafficSplitting(abTest.Variants);

            if (selectedVariant == null)
            {
                _logger.LogError("Failed to select a variant for A/B test {AbTestId} based on traffic splitting.", abTestId);
                return new UserAssignedVariantDto { Variant = null };
            }

            await AssignUserToAbTestVariantAsync(userId, abTestId, selectedVariant.Id.ToString());

            // Map AbTestVariant to VariantDto directly
            var variantDto = new VariantDto
            {
                Id = selectedVariant.Id,
                AbTestId = selectedVariant.AbTestId,
                Name = selectedVariant.Name,
                Content = selectedVariant.Content,
                TrafficAllocation = selectedVariant.TrafficSplitPercentage,
                IsControl = selectedVariant.IsControl,
                Views = (int)selectedVariant.Views,
                Conversions = (int)selectedVariant.Conversions,
                ConversionRate = selectedVariant.Views > 0 ? (double)selectedVariant.Conversions / (double)selectedVariant.Views : 0,
                CreatedAt = DateTime.UtcNow, // Assuming current timestamp for creation
                UpdatedAt = DateTime.UtcNow // Assuming current timestamp for update
            };

            return new UserAssignedVariantDto
            {
                Variant = _mapper.Map(variantDto) // Correctly map VariantDto to Variant using the mapper
            };
        }

        public async Task AssignUserToAbTestVariantAsync(string userId, string abTestId, string variantId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or whitespace.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(abTestId))
            {
                throw new ArgumentException("A/B Test ID cannot be null or whitespace.", nameof(abTestId));
            }

            if (string.IsNullOrWhiteSpace(variantId))
            {
                throw new ArgumentException("Variant ID cannot be null or whitespace.", nameof(variantId));
            }

            await _userRepository.AssignUserToAbTestVariantAsync(userId, Guid.Parse(abTestId), Guid.Parse(variantId));
        }

        public async Task<List<Variant>> GenerateVariantsAsync(string originalPageUrl, VariantGenerationRequest request)
        {
            if (string.IsNullOrWhiteSpace(originalPageUrl) || request == null)
            {
                throw new ArgumentException("Invalid input for variant generation.");
            }

            try
            {
                var generatedContents = await _aiService.GenerateContentVariationsAsync(request.GenerationPrompt ?? "Generate variations", request.NumberOfVariants);

                var variants = new List<Variant>();
                foreach (var content in generatedContents)
                {
                    var variant = new Variant(
                        name: $"AI Generated Variant - {Guid.NewGuid()}",
                        abTestId: request.AbTestId,
                        content: content,
                        trafficAllocation: 0, // Will be set later
                        isControl: false
                    );
                    variants.Add(variant);
                    await _variantRepository.AddAsync(variant);
                }

                return variants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating variants for URL {OriginalPageUrl}", originalPageUrl);
                throw;
            }
        }

        public async Task<Variant> CreateCustomVariantAsync(Guid campaignId, string name, string htmlContent, string? description = null)
        {
            if (campaignId == Guid.Empty || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Invalid input for custom variant creation.");
            }

            var variant = new Variant(
                name: name,
                abTestId: campaignId, // Assuming campaignId is the abTestId
                content: htmlContent,
                trafficAllocation: 0, // Will be set later
                isControl: false
            );

            await _variantRepository.AddAsync(variant);

            return variant;
        }

        public async Task<PageAnalysis> AnalyzePageContentAsync(string pageUrl)
        {
            // Placeholder for AI-powered page analysis
            var analysisResult = await _aiService.AnalyzePageAsync(pageUrl);
            // Convert string result to PageAnalysis object
            return new PageAnalysis
            {
                Url = pageUrl,
                PageStructure = analysisResult
            };
        }

        public async Task<List<string>> GenerateCopyVariationsAsync(string originalCopy, string targetAudience, string goal)
        {
            // Placeholder for AI-powered copy generation
            var prompt = $"Generate copy variations for: {originalCopy}. Target audience: {targetAudience}. Goal: {goal}";
            return await _aiService.GenerateCopyAsync(prompt, 5);
        }

        public async Task<List<string>> GenerateHeadlineVariationsAsync(string originalHeadline, string targetAudience, int count = 5)
        {
            // Placeholder for AI-powered headline generation
            var prompt = $"Generate headline variations for: {originalHeadline}. Target audience: {targetAudience}";
            return await _aiService.GenerateHeadlinesAsync(prompt, count);
        }

        public async Task<List<string>> GenerateCtaVariationsAsync(string originalCta, string conversionGoal, int count = 5)
        {
            // Placeholder for AI-powered CTA generation
            var prompt = $"Generate CTA variations for: {originalCta}. Conversion goal: {conversionGoal}";
            return await _aiService.GenerateCtasAsync(prompt, count);
        }

        public async Task<VariantValidationResult> ValidateVariantAsync(Variant variant)
        {
            // Placeholder for variant validation
            return await Task.FromResult(new VariantValidationResult { IsValid = true });
        }

        public async Task<IEnumerable<VariantGenerationHistory>> GetGenerationHistoryAsync(Guid campaignId)
        {
            // Placeholder for getting generation history
            return await Task.FromResult(new List<VariantGenerationHistory>());
        }

        public async Task<VariantPerformancePrediction> PredictVariantPerformanceAsync(Variant variant, PageAnalysis baselineAnalysis)
        {
            // Placeholder for performance prediction
            return await Task.FromResult(new VariantPerformancePrediction { PredictedConversionRate = 0.05 });
        }

        public async Task<Variant?> ServeVariantAsync(string userId, Guid abTestId)
        {
            var userVariant = await GetUserAbTestVariantAsync(userId, abTestId.ToString());
            return userVariant.Variant;
        }

        public async Task<bool> TrackConversionAsync(string userId, Guid variantId, decimal revenue)
        {
            var variant = await _variantRepository.GetByIdAsync(variantId);
            if (variant == null)
            {
                return false;
            }

            variant.RecordConversion();
            await _variantRepository.UpdateAsync(variant);

            // In a real implementation, you would also create a conversion event entity
            // and potentially a revenue entity if revenue is tracked.

            return true;
        }

        public async Task<VariantStatsDto?> GetVariantStatsAsync(Guid variantId)
        {
            var variant = await _variantRepository.GetByIdAsync(variantId);
            if (variant == null)
            {
                return null;
            }

            return new VariantStatsDto
            {
                VariantId = variant.Id,
                VariantName = variant.Name,
                Impressions = variant.Views,
                ConversionCount = variant.Conversions,
                ConversionRate = variant.Views > 0 ? (double)variant.Conversions / variant.Views * 100 : 0,
                TotalRevenue = 0 // TODO: Calculate actual revenue if available
            };
        }

        private AbTestVariant? SelectVariantByTrafficSplitting(IEnumerable<AbTestVariant> variants)
        {
            if (variants == null || !variants.Any())
            {
                return null;
            }

            var totalTrafficSplit = variants.Sum(v => v.TrafficSplitPercentage);
            if (totalTrafficSplit <= 0)
            {
                // Fallback to random selection if traffic splits are not set correctly
                var random = new Random();
                return variants.ElementAt(random.Next(variants.Count()));
            }

            var randomNumber = new Random().NextDouble() * (double)totalTrafficSplit;
            decimal cumulative = 0;

            foreach (var variant in variants)
            {
                cumulative += variant.TrafficSplitPercentage;
                if (randomNumber < (double)cumulative)
                {
                    return variant;
                }
            }

            return variants.LastOrDefault(); // Fallback to the last variant
        }

        public async Task<UserAssignedVariantDto?> AssignAndGetRandomVariantAsync(Guid abTestId, string userId)
        {
            var abTest = await _abTestRepository.GetByIdAsync(abTestId);
            if (abTest == null || !abTest.Variants.Any())
            {
                _logger.LogWarning("A/B test with ID {AbTestId} not found or has no variants.", abTestId);
                return null;
            }

            var selectedAbTestVariant = SelectVariantByTrafficSplitting(abTest.Variants);

            if (selectedAbTestVariant == null)
            {
                _logger.LogError("Failed to select a variant for A/B test {AbTestId} based on traffic splitting.", abTestId);
                return null;
            }

            await AssignUserToAbTestVariantAsync(userId, abTestId.ToString(), selectedAbTestVariant.Id.ToString());

            // Create a VariantDto object from AbTestVariant
            var variantDto = new VariantDto
            {
                Id = selectedAbTestVariant.Id,
                AbTestId = selectedAbTestVariant.AbTestId,
                Name = selectedAbTestVariant.Name,
                Content = selectedAbTestVariant.Content,
                TrafficAllocation = selectedAbTestVariant.TrafficSplitPercentage,
                IsControl = selectedAbTestVariant.IsControl,
                Views = (int)selectedAbTestVariant.Views,
                Conversions = (int)selectedAbTestVariant.Conversions,
                ConversionRate = selectedAbTestVariant.Views > 0 ? (double)selectedAbTestVariant.Conversions / selectedAbTestVariant.Views : 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return new UserAssignedVariantDto
            {
                Variant = _mapper.Map(variantDto) // Correctly map VariantDto to Variant using the mapper
            };
        }
    }
}