// AnalysisService.cs

using AICO.Domain.Entities;
using AICO.Domain.Interfaces;
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

        public AnalysisService(IAuditService auditService, IRecommendationService recommendationService)
        {
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
            _recommendationService = recommendationService ?? throw new ArgumentNullException(nameof(recommendationService));
        }

        /// <summary>
        /// Creates a new analysis result entity
        /// </summary>
        public AnalysisResult CreateAnalysisEntity(Guid websiteId, string analysisType, int score, string resultData, string summary)
        {
            return AnalysisResult.Create(websiteId, analysisType, score, resultData, summary);
        }

        /// <summary>
        /// Creates a new analysis result
        /// </summary>
        public async Task<AnalysisResult> CreateAnalysisAsync(Guid websiteId, string analysisType, int score, string resultData, string summary)
        {
            // Validate input is done in the entity factory method

            // Create the analysis result using the factory method
            var analysis = AnalysisResult.Create(websiteId, analysisType, score, resultData, summary);

            // Set audit information
            _auditService.SetCreationAudit(analysis);

            return analysis;
        }

        /// <summary>
        /// Updates an existing analysis result
        /// </summary>
        public async Task UpdateAnalysisAsync(AnalysisResult analysis, int newScore, string newResultData, string newSummary)
        {
            if (analysis == null)
                throw new ArgumentNullException(nameof(analysis));

            // Validate input
            if (newScore < 0 || newScore > 100)
                throw new ArgumentException("Score must be between 0 and 100", nameof(newScore));

            await ValidateAnalysisDataAsync(newResultData);

            // Create a new analysis result with updated values
            var updatedAnalysis = AnalysisResult.Create(
                analysis.WebsiteId,
                analysis.AnalysisType,
                newScore,
                newResultData,
                newSummary
            );

            // Copy the ID and recommendations from the original analysis
            typeof(BaseEntity).GetProperty("Id").SetValue(updatedAnalysis, analysis.Id);
            typeof(AnalysisResult).GetProperty("Recommendations").SetValue(updatedAnalysis, analysis.Recommendations);

            // Copy the updated values back to the original analysis
            typeof(AnalysisResult).GetProperty("Score").SetValue(analysis, newScore);
            typeof(AnalysisResult).GetProperty("ResultData").SetValue(analysis, newResultData);
            typeof(AnalysisResult).GetProperty("Summary").SetValue(analysis, newSummary);

            // Update audit information
            _auditService.UpdateModificationDate(analysis);
        }

        /// <summary>
        /// Validates analysis data
        /// </summary>
        public async Task<bool> ValidateAnalysisDataAsync(string resultData)
        {
            if (string.IsNullOrWhiteSpace(resultData))
                return true;

            try
            {
                // Try to parse the JSON data to validate it
                JsonDocument.Parse(resultData);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a recommendation to an analysis result
        /// </summary>
        public async Task AddRecommendationAsync(AnalysisResult analysis, string title, string description, int priority, string category)
        {
            if (analysis == null)
                throw new ArgumentNullException(nameof(analysis));

            // Create the recommendation using the recommendation service
            var recommendation = await _recommendationService.CreateRecommendationAsync(
                analysis.Id,
                title,
                description,
                priority,
                category
            );

            // Create a new collection with the added recommendation
            var updatedRecommendations = new List<Recommendation>(analysis.Recommendations) { recommendation };

            // Update the recommendations collection using reflection
            typeof(AnalysisResult).GetProperty("Recommendations").SetValue(analysis, updatedRecommendations);

            // Update audit information for the analysis
            _auditService.UpdateModificationDate(analysis);
        }

        /// <summary>
        /// Validates analysis input
        /// </summary>
        private async Task ValidateAnalysisInputAsync(string analysisType, int score, string resultData)
        {
            if (string.IsNullOrWhiteSpace(analysisType))
                throw new ArgumentException("Analysis type cannot be empty", nameof(analysisType));

            if (score < 0 || score > 100)
                throw new ArgumentException("Score must be between 0 and 100", nameof(score));

            if (!await ValidateAnalysisDataAsync(resultData))
                throw new ArgumentException("Invalid JSON data", nameof(resultData));
        }

        /// <summary>
        /// Validates recommendation input
        /// </summary>
        private Task ValidateRecommendationInputAsync(string title, int priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            if (priority < 1 || priority > 5)
                throw new ArgumentException("Priority must be between 1 and 5", nameof(priority));

            return Task.CompletedTask;
        }

        public Task<AnalysisResult> AnalyzeWebsiteAsync(Guid websiteId, string analysisType)
        {
            throw new NotImplementedException();
        }

        public Task<AnalysisResult> GetAnalysisResultByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AnalysisResult>> GetAnalysisResultsByWebsiteIdAsync(Guid websiteId)
        {
            throw new NotImplementedException();
        }

        public Task<AnalysisResult> GetLatestAnalysisResultAsync(Guid websiteId)
        {
            throw new NotImplementedException();
        }

        public Task<Recommendation> AddRecommendationAsync(Guid analysisResultId, string title, string description, int priority, string category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Recommendation>> GetRecommendationsByAnalysisResultIdAsync(Guid analysisResultId)
        {
            throw new NotImplementedException();
        }

        public Task MarkRecommendationAsImplementedAsync(Guid recommendationId)
        {
            throw new NotImplementedException();
        }

        public Task MarkRecommendationAsNotImplementedAsync(Guid recommendationId)
        {
            throw new NotImplementedException();
        }
    }
}