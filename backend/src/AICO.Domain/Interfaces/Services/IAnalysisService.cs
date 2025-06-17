// IAnalysisService.cs

using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for handling analysis-related operations
    /// </summary>
    public interface IAnalysisService
    {
        /// <summary>
        /// Creates a new analysis result entity. This is typically a synchronous factory method.
        /// </summary>
        AnalysisResult CreateAnalysisEntity(Guid websiteId, string analysisType, int score, string resultData, string summary);

        /// <summary>
        /// Creates a new analysis result asynchronously.
        /// </summary>
        Task<AnalysisResult> CreateAnalysisAsync(Guid websiteId, string analysisType, int score, string resultData, string summary);

        /// <summary>
        /// Updates an existing analysis result asynchronously.
        /// </summary>
        Task UpdateAnalysisAsync(AnalysisResult analysis, int newScore, string newResultData, string newSummary);

        /// <summary>
        /// Validates analysis data asynchronously.
        /// </summary>
        Task<bool> ValidateAnalysisDataAsync(string resultData);

        /// <summary>
        /// Analyzes a website asynchronously.
        /// </summary>
        Task<AnalysisResult> AnalyzeWebsiteAsync(Guid websiteId, string analysisType);

        /// <summary>
        /// Gets an analysis result by ID asynchronously.
        /// </summary>
        Task<AnalysisResult> GetAnalysisResultByIdAsync(Guid id);

        /// <summary>
        /// Gets all analysis results for a website asynchronously.
        /// </summary>
        Task<IEnumerable<AnalysisResult>> GetAnalysisResultsByWebsiteIdAsync(Guid websiteId);

        /// <summary>
        /// Gets the latest analysis result for a website asynchronously.
        /// </summary>
        Task<AnalysisResult> GetLatestAnalysisResultAsync(Guid websiteId);

        /// <summary>
        /// Adds a recommendation to an analysis result asynchronously.
        /// </summary>
        Task<Recommendation> AddRecommendationAsync(Guid analysisResultId, string title, string description, int priority, string category);

        /// <summary>
        /// Gets recommendations for an analysis result asynchronously.
        /// </summary>
        Task<IEnumerable<Recommendation>> GetRecommendationsByAnalysisResultIdAsync(Guid analysisResultId);

        /// <summary>
        /// Marks a recommendation as implemented asynchronously.
        /// </summary>
        Task MarkRecommendationAsImplementedAsync(Guid recommendationId);

        /// <summary>
        /// Marks a recommendation as not implemented asynchronously.
        /// </summary>
        Task MarkRecommendationAsNotImplementedAsync(Guid recommendationId);
    }
}
