using System;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces
{
    /// <summary>
    /// Service for handling analysis-related operations
    /// </summary>
    public interface IAnalysisService
    {
        /// <summary>
        /// Creates a new analysis result
        /// </summary>
        Task<AnalysisResult> CreateAnalysisAsync(Guid websiteId, string analysisType, int score, string resultData, string summary);
        
        /// <summary>
        /// Updates an existing analysis result
        /// </summary>
        Task UpdateAnalysisAsync(AnalysisResult analysis, int newScore, string newResultData, string newSummary);
        
        /// <summary>
        /// Validates analysis data
        /// </summary>
        Task<bool> ValidateAnalysisDataAsync(string resultData);
        
        /// <summary>
        /// Adds a recommendation to an analysis result
        /// </summary>
        Task AddRecommendationAsync(AnalysisResult analysis, string title, string description, int priority, string category);
        
        /// <summary>
        /// Creates a new analysis result entity
        /// </summary>
        AnalysisResult CreateAnalysisEntity(Guid websiteId, string analysisType, int score, string resultData, string summary);
    }
} 