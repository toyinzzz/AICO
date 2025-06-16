// IAnalysisService.cs

using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service interface for Analysis-related business logic
    /// </summary>
    public interface IAnalysisService
    {
        /// <summary>
        /// Initiates a new analysis for a website
        /// </summary>
        Task<AnalysisResult> AnalyzeWebsiteAsync(Guid websiteId, string analysisType);

        /// <summary>
        /// Gets an analysis result by ID
        /// </summary>
        Task<AnalysisResult> GetAnalysisResultByIdAsync(Guid id);

        /// <summary>
        /// Gets all analysis results for a website
        /// </summary>
        Task<IEnumerable<AnalysisResult>> GetAnalysisResultsByWebsiteIdAsync(Guid websiteId);

        /// <summary>
        /// Gets the latest analysis result for a website
        /// </summary>
        Task<AnalysisResult> GetLatestAnalysisResultAsync(Guid websiteId);

        /// <summary>
        /// Adds a recommendation to an analysis result
        /// </summary>
        Task<Recommendation> AddRecommendationAsync(Guid analysisResultId, string title, string description, int priority, string category);

        /// <summary>
        /// Gets all recommendations for an analysis result
        /// </summary>
        Task<IEnumerable<Recommendation>> GetRecommendationsByAnalysisResultIdAsync(Guid analysisResultId);

        /// <summary>
        /// Marks a recommendation as implemented
        /// </summary>
        Task MarkRecommendationAsImplementedAsync(Guid recommendationId);

        /// <summary>
        /// Marks a recommendation as not implemented
        /// </summary>
        Task MarkRecommendationAsNotImplementedAsync(Guid recommendationId);
        Task <AnalysisResult> CreateAnalysisEntity(Guid websiteId, string analysisType, int score, string resultData, string summary);
        Task<AnalysisResult> CreateAnalysisAsync(Guid websiteId, string analysisType, int score, string resultData, string summary);
        Task UpdateAnalysisAsync(AnalysisResult analysis, int newScore, string newResultData, string newSummary);
        Task<bool> ValidateAnalysisDataAsync(string validJson);
    }
}
