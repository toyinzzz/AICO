using AICO.Domain.Entities;
using AICO.Domain.Interfaces;

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for handling recommendation-related operations
    /// </summary>
    public class RecommendationService : IRecommendationService
    {
        private readonly IAuditService _auditService;

        public RecommendationService(IAuditService auditService)
        {
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
        }

        /// <summary>
        /// Creates a new recommendation entity
        /// </summary>
        public Recommendation CreateRecommendationEntity(Guid analysisResultId, string title, string description, int priority, string category)
        {
            return Recommendation.Create(analysisResultId, title, description, priority, category);
        }

        /// <summary>
        /// Creates a new recommendation
        /// </summary>
        public async Task<Recommendation> CreateRecommendationAsync(Guid analysisResultId, string title, string description, int priority, string category)
        {
            // Validation is done in the entity factory method

            // Create the recommendation using the factory method
            var recommendation = Recommendation.Create(analysisResultId, title, description, priority, category);

            // Set audit information
            _auditService.SetCreationAudit(recommendation);

            return recommendation;
        }

        /// <summary>
        /// Updates an existing recommendation
        /// </summary>
        public async Task UpdateRecommendationAsync(Recommendation recommendation, string title, string description, int priority, string category)
        {
            if (recommendation == null)
                throw new ArgumentNullException(nameof(recommendation));

            // Validate inputs
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required", nameof(title));
            
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required", nameof(description));
            
            if (priority < 1 || priority > 5)
                throw new ArgumentException("Priority must be between 1 and 5", nameof(priority));

            // Update the recommendation using safe internal methods (no reflection needed)
            recommendation.UpdateDetails(title, description, priority, category);
            
            // Update audit information
            _auditService.UpdateModificationDate(recommendation);
            
            // Note: Persistence should be handled by the calling application service
        }

        /// <summary>
        /// Marks a recommendation as implemented
        /// </summary>
        public Task MarkAsImplementedAsync(Recommendation recommendation)
        {
            if (recommendation == null)
                throw new ArgumentNullException(nameof(recommendation));

            // Create a new implemented recommendation
            var implementedRecommendation = Recommendation.CreateImplemented(
                recommendation.AnalysisResultId,
                recommendation.Title,
                recommendation.Description,
                recommendation.Priority,
                recommendation.RecommendationType,
                DateTime.UtcNow
            );

            // Copy the ID from the original recommendation
            typeof(BaseEntity).GetProperty("Id").SetValue(implementedRecommendation, recommendation.Id);

            // Copy the implementation status to the original recommendation
            typeof(Recommendation).GetProperty("IsImplemented").SetValue(recommendation, true);
            typeof(Recommendation).GetProperty("ImplementedAt").SetValue(recommendation, DateTime.UtcNow);

            // Update audit information
            _auditService.UpdateModificationDate(recommendation);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Marks a recommendation as not implemented
        /// </summary>
        public Task MarkAsNotImplementedAsync(Recommendation recommendation)
        {
            if (recommendation == null)
                throw new ArgumentNullException(nameof(recommendation));

            // Create a new non-implemented recommendation
            var nonImplementedRecommendation = Recommendation.Create(
                recommendation.AnalysisResultId,
                recommendation.Title,
                recommendation.Description,
                recommendation.Priority,
                recommendation.RecommendationType
            );

            // Copy the ID from the original recommendation
            typeof(BaseEntity).GetProperty("Id").SetValue(nonImplementedRecommendation, recommendation.Id);

            // Copy the implementation status to the original recommendation
            typeof(Recommendation).GetProperty("IsImplemented").SetValue(recommendation, false);
            typeof(Recommendation).GetProperty("ImplementedAt").SetValue(recommendation, null);

            // Update audit information
            _auditService.UpdateModificationDate(recommendation);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Validates recommendation data
        /// </summary>
        public Task<bool> ValidateRecommendationDataAsync(string title, int priority)
        {
            try
            {
                // Validation is done by trying to create a dummy recommendation
                Recommendation.Create(Guid.Empty, title, null, priority, null);
                return Task.FromResult(true);
            }
            catch (ArgumentException)
            {
                return Task.FromResult(false);
            }
        }
    }
}