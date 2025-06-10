// Website.cs

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a website that can be analyzed
    /// </summary>
    public class Website : BaseEntity
    {
        /// <summary>
        /// The URL of the website
        /// </summary>
        [Required]
        [Url]
        [MaxLength(2048)]
        public string Url { get; private set; }

        /// <summary>
        /// The name of the website
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; private set; }

        /// <summary>
        /// Description of the website
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; private set; }

        /// <summary>
        /// The industry or category the website belongs to
        /// </summary>
        [MaxLength(100)]
        public string Industry { get; private set; }

        /// <summary>
        /// The ID of the user who owns this website
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Navigation property to the user who owns this website
        /// </summary>
        public virtual User User { get; private set; }

        /// <summary>
        /// Collection of analysis results for this website
        /// </summary>
        public virtual ICollection<AnalysisResult> AnalysisResults { get; private set; }

        /// <summary>
        /// Date of the last analysis
        /// </summary>
        public DateTime? LastAnalyzedAt { get; private set; }

        // Private constructor for EF Core
        private Website()
        {
            AnalysisResults = new List<AnalysisResult>();
        }

        // Static factory method for creating a website
        public static Website Create(
            string url,
            string name,
            Guid userId,
            string description = null,
            string industry = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL is required", nameof(url));
            
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));
            
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required", nameof(userId));

            return new Website
            {
                Url = url.Trim(),
                Name = name.Trim(),
                UserId = userId,
                Description = description?.Trim(),
                Industry = industry?.Trim()
            };
        }

        // Method for updating website information
        internal void Update(string name, string description, string industry)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            Name = name.Trim();
            Description = description?.Trim();
            Industry = industry?.Trim();
        }

        // Method for updating the URL
        internal void UpdateUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL is required", nameof(url));

            Url = url.Trim();
        }

        // Method for updating the last analysis date
        internal void UpdateLastAnalyzedAt()
        {
            LastAnalyzedAt = DateTime.UtcNow;
        }
    }
}
 