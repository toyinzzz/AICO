// Website.cs

using System.ComponentModel.DataAnnotations;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a website that can be analyzed
    /// </summary>
    public class Website : BaseEntity
    {
        public string Url { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Industry { get; private set; }
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<AnalysisResult> AnalysisResults { get; private set; }
        public DateTime? LastAnalyzedAt { get; private set; }

        // Add the missing Domain property  
        public string Domain { get; private set; }

        public static Website Create(string url, string name, Guid userId, string description = null, string industry = null, string domain = null)
        {
            return new Website
            {
                Url = url,
                Name = name,
                UserId = userId,
                Description = description,
                Industry = industry,
                Domain = domain
            };
        }

        internal void Update(string name, string description, string industry, string domain)
        {
            Name = name;
            Description = description;
            Industry = industry;
            Domain = domain;
        }

        internal void UpdateUrl(string url)
        {
            Url = url;
        }

        internal void UpdateLastAnalyzedAt()
        {
            LastAnalyzedAt = DateTime.UtcNow;
        }
    }
}
