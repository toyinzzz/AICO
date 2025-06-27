using AICO.Domain.Entities;
using AICO.Domain.Interfaces;
using AICO.Domain.Interfaces.Services;

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
        public Recommendation CreateRecommendationEntity(Guid analysisResultId, string title, string description, int priority, string recommendationType)
        {
            return Recommendation.Create(analysisResultId, title, description, priority, recommendationType);
        }

        /// <summary>
        /// Creates a new recommendation
        /// </summary>
        public async Task<Recommendation> CreateRecommendationAsync(Guid analysisResultId, string title, string description, int priority, string recommendationType)
        {
            // Validation is done in the entity factory method

            // Create the recommendation using the factory method
            var recommendation = Recommendation.Create(analysisResultId, title, description, priority, recommendationType);

            // Set audit information
            _auditService.SetCreationAudit(recommendation, null); // Pass null for userId

            return recommendation;
        }

        /// <summary>
        /// Updates an existing recommendation
        /// </summary>
        public async Task UpdateRecommendationAsync(Recommendation recommendation, string title, string description, int priority, string recommendationType)
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

            // Update the recommendation properties directly
            recommendation.UpdateTitle(title);
            recommendation.UpdateDescription(description);
            recommendation.UpdatePriority(priority);
            recommendation.UpdateRecommendationType(recommendationType);
            
            // Update audit information
            _auditService.UpdateModificationDate(recommendation, null); // Explicitly pass null for userId
            
            // Note: Persistence should be handled by the calling application service
        }

        /// <summary>
        /// Marks a recommendation as implemented
        /// </summary>
        public Task MarkAsImplementedAsync(Recommendation recommendation)
        {
            if (recommendation == null)
                throw new ArgumentNullException(nameof(recommendation));

            recommendation.MarkAsImplemented();

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

            recommendation.MarkAsPending();

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
                // Assuming a general category for validation purposes
                Recommendation.Create(Guid.NewGuid(), title, "Validation Description", priority, "General");
                return Task.FromResult(true);
            }
            catch (ArgumentException)
            {
                return Task.FromResult(false);
            }
        }
    }
}