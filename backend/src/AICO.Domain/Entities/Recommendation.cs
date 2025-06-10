// Recommendation.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a recommendation based on an analysis result
    /// </summary>
    public class Recommendation : BaseEntity
    {
        /// <summary>
        /// The ID of the analysis result this recommendation belongs to
        /// </summary>
        public Guid AnalysisResultId { get; private set; }

        /// <summary>
        /// Navigation property to the analysis result
        /// </summary>
        public virtual AnalysisResult AnalysisResult { get; private set; }

        /// <summary>
        /// The title of the recommendation
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; private set; }

        /// <summary>
        /// The detailed description of the recommendation
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The priority of the recommendation (1-5, where 1 is highest)
        /// </summary>
        [Range(1, 5)]
        public int Priority { get; private set; }

        /// <summary>
        /// The category of the recommendation (e.g., SEO, Performance)
        /// </summary>
        [MaxLength(100)]
        public string Category { get; private set; }

        /// <summary>
        /// Flag indicating if the recommendation has been implemented
        /// </summary>
        public bool IsImplemented { get; private set; }

        /// <summary>
        /// Date when the recommendation was implemented
        /// </summary>
        public DateTime? ImplementedAt { get; private set; }

        // Private constructor for EF Core
        private Recommendation()
        {
        }

        // Static factory method for controlled creation
        public static Recommendation Create(
            Guid analysisResultId,
            string title,
            string description,
            int priority,
            string category)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
            
            if (priority < 1 || priority > 5)
                throw new ArgumentOutOfRangeException(nameof(priority), "Priority must be between 1 and 5");

            return new Recommendation
            {
                AnalysisResultId = analysisResultId,
                Title = title,
                Description = description,
                Priority = priority,
                Category = category,
                IsImplemented = false,
                ImplementedAt = null
            };
        }

        // Static factory method for creating an implemented recommendation
        public static Recommendation CreateImplemented(
            Guid analysisResultId,
            string title,
            string description,
            int priority,
            string category,
            DateTime implementedAt)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
            
            if (priority < 1 || priority > 5)
                throw new ArgumentOutOfRangeException(nameof(priority), "Priority must be between 1 and 5");

            return new Recommendation
            {
                AnalysisResultId = analysisResultId,
                Title = title,
                Description = description,
                Priority = priority,
                Category = category,
                IsImplemented = true,
                ImplementedAt = implementedAt
            };
        }
    }
}
