using System.ComponentModel.DataAnnotations;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a JavaScript snippet for website integration
    /// </summary>
    public class Snippet : BaseEntity
    {
        /// <summary>
        /// Website this snippet belongs to
        /// </summary>
        [Required]
        public Guid WebsiteId { get; private set; }
        public Website Website { get; private set; }

        /// <summary>
        /// Snippet name/identifier
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; private set; }

        /// <summary>
        /// JavaScript code for the snippet
        /// </summary>
        [Required]
        public string Code { get; private set; }

        /// <summary>
        /// Snippet version
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Version { get; private set; } = "1.0.0";

        /// <summary>
        /// Whether the snippet is active
        /// </summary>
        public bool IsActive { get; private set; } = true;

        /// <summary>
        /// Snippet type (e.g., "tracking", "ab-test", "analytics")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Type { get; private set; }

        /// <summary>
        /// Configuration settings as JSON
        /// </summary>
        public string Configuration { get; private set; }

        /// <summary>
        /// Last time the snippet was loaded
        /// </summary>
        public DateTime? LastLoadedAt { get; private set; }

        /// <summary>
        /// Number of times the snippet has been loaded
        /// </summary>
        public int LoadCount { get; private set; }

        /// <summary>
        /// Private constructor for EF Core
        /// </summary>
        private Snippet() { }

        /// <summary>
        /// Creates a new snippet
        /// </summary>
        public Snippet(Guid websiteId, string name, string code, string type,
                      string version = "1.0.0", string configuration = null)
        {
            WebsiteId = websiteId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Version = version ?? "1.0.0";
            Configuration = configuration;
        }

        /// <summary>
        /// Updates the snippet code
        /// </summary>
        public void UpdateCode(string newCode, string newVersion = null)
        {
            Code = newCode ?? throw new ArgumentNullException(nameof(newCode));
            if (!string.IsNullOrEmpty(newVersion))
                Version = newVersion;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Activates the snippet
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Deactivates the snippet
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Records a snippet load
        /// </summary>
        public void RecordLoad()
        {
            LoadCount++;
            LastLoadedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}