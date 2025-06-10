// AnalysisResult.cs

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents the result of a website analysis
    /// </summary>
    public class AnalysisResult : BaseEntity
    {
        /// <summary>
        /// The ID of the website that was analyzed
        /// </summary>
        public Guid WebsiteId { get; private set; }

        /// <summary>
        /// Navigation property to the website that was analyzed
        /// </summary>
        public virtual Website Website { get; private set; }

        /// <summary>
        /// The type of analysis performed (e.g., SEO, Performance, Accessibility)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string AnalysisType { get; private set; }

        /// <summary>
        /// The overall score of the analysis (0-100)
        /// </summary>
        [Range(0, 100)]
        public int Score { get; private set; }

        /// <summary>
        /// Serialized JSON data containing the detailed analysis results
        /// </summary>
        [Required]
        public string ResultData { get; private set; }

        /// <summary>
        /// Summary of the analysis results
        /// </summary>
        public string Summary { get; private set; }

        /// <summary>
        /// Collection of recommendations based on the analysis
        /// </summary>
        public virtual ICollection<Recommendation> Recommendations { get; private set; }

        // Private constructor for EF Core
        private AnalysisResult()
        {
            Recommendations = new List<Recommendation>();
        }

        // Static factory method for controlled creation
        public static AnalysisResult Create(
            Guid websiteId,
            string analysisType,
            int score,
            string resultData,
            string summary = null)
        {
            if (string.IsNullOrWhiteSpace(analysisType))
                throw new ArgumentException("Analysis type cannot be null or empty", nameof(analysisType));
            
            if (string.IsNullOrWhiteSpace(resultData))
                throw new ArgumentException("Result data cannot be null or empty", nameof(resultData));
            
            if (score < 0 || score > 100)
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100");

            return new AnalysisResult
            {
                WebsiteId = websiteId,
                AnalysisType = analysisType,
                Score = score,
                ResultData = resultData,
                Summary = summary,
                Recommendations = new List<Recommendation>()
            };
        }
    }
}