using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces
{
    /// <summary>
    /// Service for handling recommendation-related operations
    /// </summary>
    public interface IRecommendationService
    {
        /// <summary>
        /// Creates a new recommendation
        /// </summary>
        Task<Recommendation> CreateRecommendationAsync(Guid analysisResultId, string title, string description, int priority, string category);

        /// <summary>
        /// Updates an existing recommendation
        /// </summary>
        Task UpdateRecommendationAsync(Recommendation recommendation, string title, string description, int priority, string category);

        /// <summary>
        /// Marks a recommendation as implemented
        /// </summary>
        Task MarkAsImplementedAsync(Recommendation recommendation);

        /// <summary>
        /// Marks a recommendation as not implemented
        /// </summary>
        Task MarkAsNotImplementedAsync(Recommendation recommendation);

        /// <summary>
        /// Validates recommendation data
        /// </summary>
        Task<bool> ValidateRecommendationDataAsync(string title, int priority);

        /// <summary>
        /// Creates a new recommendation entity
        /// </summary>
        Recommendation CreateRecommendationEntity(Guid analysisResultId, string title, string description, int priority, string category);
    }
}