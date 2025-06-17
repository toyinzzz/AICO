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
        /// Creates a new analysis result, performing asynchronous operations if necessary.
        /// </summary>
        public async Task<AnalysisResult> CreateAnalysisAsync(Guid websiteId, string analysisType, int score, string resultData, string summary)
        {
            ValidateAnalysisInput(analysisType, score, resultData); // Keep this validation for now
            var analysis = AnalysisResult.Create(websiteId, analysisType, score, resultData, summary);
            _auditService.SetCreationAudit(analysis); // Assuming this can be called in an async context
            return await Task.FromResult(analysis); // Correctly return a Task<AnalysisResult>
        }

        /// <summary>
        /// Updates an existing analysis result, performing asynchronous operations if necessary.
        /// </summary>
        public async Task UpdateAnalysisAsync(AnalysisResult analysis, int newScore, string newResultData, string newSummary)
        {
            if (analysis == null) throw new ArgumentNullException(nameof(analysis));
            if (newScore < 0 || newScore > 100) throw new ArgumentOutOfRangeException(nameof(newScore), "Score must be between 0 and 100");
            if (!await ValidateAnalysisDataAsync(newResultData)) throw new ArgumentException("Invalid JSON data", nameof(newResultData));

            analysis.Score = newScore;
            analysis.ResultData = newResultData;
            analysis.Summary = newSummary;
            _auditService.UpdateModificationDate(analysis); // Assuming this can be called in an async context
            // No explicit return needed for Task method if all paths are async or complete
        }

        /// <summary>
        /// Validates analysis data asynchronously.
        /// </summary>
        public async Task<bool> ValidateAnalysisDataAsync(string resultData)
        {
            if (string.IsNullOrWhiteSpace(resultData))
                return await Task.FromResult(true);
            try
            {
                JsonDocument.Parse(resultData);
                return await Task.FromResult(true);
            }
            catch (JsonException)
            {
                return await Task.FromResult(false);
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

            // Ensure Recommendations collection is initialized
            var recommendations = analysis.Recommendations as ICollection<Recommendation> ?? new List<Recommendation>();
            recommendations.Add(recommendation);

            // Update the recommendations collection using reflection (consider a public setter or method on AnalysisResult)
            typeof(AnalysisResult).GetProperty("Recommendations").SetValue(analysis, recommendations);

            // Update audit information for the analysis
            _auditService.UpdateModificationDate(analysis);
        }

        /// <summary>
        /// Validates analysis input
        /// </summary>
        private void ValidateAnalysisInput(string analysisType, int score, string resultData) // Made synchronous
        {
            if (string.IsNullOrWhiteSpace(analysisType))
                throw new ArgumentException("Analysis type cannot be empty", nameof(analysisType));

            if (score < 0 || score > 100)
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100"); // Changed to ArgumentOutOfRangeException

            if (!ValidateAnalysisDataAsync(resultData).Result) // Call async version and get result
                throw new ArgumentException("Invalid JSON data", nameof(resultData));
        }

        /// <summary>
        /// Validates recommendation input
        /// </summary>
        private void ValidateRecommendationInput(string title, int priority) // Made synchronous
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            if (priority < 1 || priority > 5)
                throw new ArgumentException("Priority must be between 1 and 5", nameof(priority));
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

        // Remove incorrect explicit interface implementations below
        // Task<AnalysisResult> IAnalysisService.CreateAnalysisEntity(...) NO LONGER NEEDED due to public sync method

        // Task<AnalysisResult> IAnalysisService.CreateAnalysisAsync(...) NO LONGER NEEDED if public async method matches

        // Task IAnalysisService.UpdateAnalysisAsync(...) NO LONGER NEEDED if public async method matches

        // Task<bool> IAnalysisService.ValidateAnalysisDataAsync(...) NO LONGER NEEDED if public async method matches

    }
}