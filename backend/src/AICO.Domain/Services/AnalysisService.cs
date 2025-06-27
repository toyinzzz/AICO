// AnalysisService.cs

using AICO.Domain.Entities;
using AICO.Domain.Interfaces;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using System.Text.Json;

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for handling analysis-related operations
    /// </summary>
    public class AnalysisService : IAnalysisService
    {
        private readonly IAuditService _auditService;
        private readonly IRecommendationService _recommendationService;
        private readonly IAnalysisResultRepository _analysisResultRepository;
        private readonly IRecommendationRepository _recommendationRepository;

        public AnalysisService(
            IAuditService auditService,
            IRecommendationService recommendationService,
            IAnalysisResultRepository analysisResultRepository,
            IRecommendationRepository recommendationRepository)
        {
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
            _recommendationService = recommendationService ?? throw new ArgumentNullException(nameof(recommendationService));
            _analysisResultRepository = analysisResultRepository ?? throw new ArgumentNullException(nameof(analysisResultRepository));
            _recommendationRepository = recommendationRepository ?? throw new ArgumentNullException(nameof(recommendationRepository));
        }

        /// <summary>
        /// Creates a new analysis result entity. This is typically a synchronous factory method.
        /// </summary>
        public AnalysisResult CreateAnalysisEntity(Guid websiteId, string analysisType, int score, string resultData, string summary)
        {
            return AnalysisResult.Create(websiteId, analysisType, score, resultData, summary);
        }

        /// <summary>
        /// Creates a new analysis result asynchronously.
        /// </summary>
        public async Task<AnalysisResult> CreateAnalysisAsync(Guid websiteId, string analysisType, int score, string resultData, string summary)
        {
            ValidateAnalysisInput(analysisType, score, resultData);
            var analysis = AnalysisResult.Create(websiteId, analysisType, score, resultData, summary);
            _auditService.SetCreationAudit(analysis);
            await _analysisResultRepository.AddAsync(analysis);
            return analysis;
        }

        /// <summary>
        /// Updates an existing analysis result asynchronously.
        /// </summary>
        public async Task UpdateAnalysisAsync(AnalysisResult analysis, int newScore, string newResultData, string newSummary)
        {
            if (analysis == null) throw new ArgumentNullException(nameof(analysis));
            if (newScore < 0 || newScore > 100) throw new ArgumentOutOfRangeException(nameof(newScore), "Score must be between 0 and 100");
            if (!await ValidateAnalysisDataAsync(newResultData)) throw new ArgumentException("Invalid JSON data", nameof(newResultData));

            analysis.Score = newScore;
            analysis.ResultData = newResultData;
            analysis.Summary = newSummary;
            _auditService.UpdateModificationDate(analysis);
            await _analysisResultRepository.UpdateAsync(analysis);
        }

        /// <summary>
        /// Validates analysis data asynchronously.
        /// </summary>
        public async Task<bool> ValidateAnalysisDataAsync(string resultData)
        {
            if (string.IsNullOrWhiteSpace(resultData))
                return true;
            try
            {
                JsonDocument.Parse(resultData);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Analyzes a website asynchronously (Dummy-Implementierung).
        /// </summary>
        public async Task<AnalysisResult> AnalyzeWebsiteAsync(Guid websiteId, string analysisType)
        {
            var resultData = "{\"status\":\"ok\"}";
            var summary = $"Analysis of type {analysisType} completed.";
            var score = 80;
            var analysis = AnalysisResult.Create(websiteId, analysisType, score, resultData, summary);
            _auditService.SetCreationAudit(analysis);
            await _analysisResultRepository.AddAsync(analysis);
            return analysis;
        }

        /// <summary>
        /// Gets an analysis result by ID asynchronously.
        /// </summary>
        public async Task<AnalysisResult> GetAnalysisResultByIdAsync(Guid id)
        {
            return await _analysisResultRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets all analysis results for a website asynchronously.
        /// </summary>
        public async Task<IEnumerable<AnalysisResult>> GetAnalysisResultsByWebsiteIdAsync(Guid websiteId)
        {
            return await _analysisResultRepository.GetByWebsiteIdAsync(websiteId);
        }

        /// <summary>
        /// Gets the latest analysis result for a website asynchronously.
        /// </summary>
        public async Task<AnalysisResult> GetLatestAnalysisResultAsync(Guid websiteId)
        {
            return await _analysisResultRepository.GetLatestByWebsiteIdAsync(websiteId);
        }

        // Fix for CS0272: Use a method to add recommendations instead of directly setting the property.
        // Fix for IDE0028: Simplify collection initialization using object initializer syntax.

        /// <summary>
        /// Adds a recommendation to an analysis result asynchronously.
        /// </summary>
        public async Task<Recommendation> AddRecommendationAsync(Guid analysisResultId, string title, string description, int priority, string category)
        {
            var analysis = await _analysisResultRepository.GetByIdAsync(analysisResultId);
            if (analysis == null)
                throw new ArgumentException("AnalysisResult not found.", nameof(analysisResultId));

            ValidateRecommendationInput(title, priority);

            var recommendation = await _recommendationService.CreateRecommendationAsync(
                analysis.Id,
                title,
                description,
                priority,
                category
            );

            // Ensure Recommendations collection is initialized and add the recommendation
            if (analysis.Recommendations == null)
            {
                var recommendations = new List<Recommendation> { recommendation };
                analysis.GetType().GetProperty(nameof(AnalysisResult.Recommendations))?.SetValue(analysis, recommendations);
            }
            else
            {
                analysis.Recommendations.Add(recommendation);
            }

            _auditService.UpdateModificationDate(analysis);
            await _analysisResultRepository.UpdateAsync(analysis);
            await _recommendationRepository.AddAsync(recommendation);

            return recommendation;
        }

        /// <summary>
        /// Gets recommendations for an analysis result asynchronously.
        /// </summary>
        public async Task<IEnumerable<Recommendation>> GetRecommendationsByAnalysisResultIdAsync(Guid analysisResultId)
        {
            return await _recommendationRepository.GetByAnalysisResultIdAsync(analysisResultId);
        }

        /// <summary>
        /// Marks a recommendation as implemented asynchronously.
        /// </summary>
        public async Task MarkRecommendationAsImplementedAsync(Guid recommendationId)
        {
            var recommendation = await _recommendationRepository.GetByIdAsync(recommendationId);
            if (recommendation == null)
                throw new ArgumentException("Recommendation not found", nameof(recommendationId));
            recommendation.MarkAsImplemented();
            await _recommendationRepository.UpdateAsync(recommendation);
        }

        /// <summary>
        /// Marks a recommendation as not implemented asynchronously.
        /// </summary>
        public async Task MarkRecommendationAsNotImplementedAsync(Guid recommendationId)
        {
            var recommendation = await _recommendationRepository.GetByIdAsync(recommendationId);
            if (recommendation == null)
                throw new ArgumentException("Recommendation not found", nameof(recommendationId));
            recommendation.MarkAsPending();
            await _recommendationRepository.UpdateAsync(recommendation);
        }

        /// <summary>
        /// Validates analysis input (internal helper).
        /// </summary>
        private void ValidateAnalysisInput(string analysisType, int score, string resultData)
        {
            if (string.IsNullOrWhiteSpace(analysisType))
                throw new ArgumentException("Analysis type cannot be empty", nameof(analysisType));

            if (score < 0 || score > 100)
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100");

            if (!ValidateAnalysisDataAsync(resultData).Result)
                throw new ArgumentException("Invalid JSON data", nameof(resultData));
        }

        /// <summary>
        /// Validates recommendation input (internal helper).
        /// </summary>
        private void ValidateRecommendationInput(string title, int priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            if (priority < 1 || priority > 5)
                throw new ArgumentException("Priority must be between 1 and 5", nameof(priority));
        }
    }
}