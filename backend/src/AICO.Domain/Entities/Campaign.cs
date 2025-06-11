using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AICO.Domain.Services;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents an A/B testing campaign
    /// </summary>
    public class Campaign : BaseEntity
    {
        /// <summary>
        /// Campaign name
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; private set; }

        /// <summary>
        /// Campaign description
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; private set; }

        /// <summary>
        /// Website this campaign belongs to
        /// </summary>
        [Required]
        public Guid WebsiteId { get; private set; }
        public Website Website { get; private set; }

        /// <summary>
        /// User who created this campaign
        /// </summary>
        [Required]
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        /// <summary>
        /// Campaign status
        /// </summary>
        [Required]
        public CampaignStatus Status { get; private set; }

        /// <summary>
        /// Campaign start date
        /// </summary>
        public DateTime? StartDate { get; private set; }

        /// <summary>
        /// Campaign end date
        /// </summary>
        public DateTime? EndDate { get; private set; }

        /// <summary>
        /// Traffic allocation percentage (0-100)
        /// </summary>
        [Range(0, 100)]
        public int TrafficAllocation { get; private set; } = 50;

        /// <summary>
        /// A/B tests in this campaign
        /// </summary>
        public ICollection<AbTest> AbTests { get; private set; } = new List<AbTest>();

        /// <summary>
        /// Private constructor for EF Core
        /// </summary>
        private Campaign() { }

        /// <summary>
        /// Creates a new campaign
        /// </summary>
        public Campaign(string name, string description, Guid websiteId, Guid userId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            WebsiteId = websiteId;
            UserId = userId;
            Status = CampaignStatus.Draft;
        }

        /// <summary>
        /// Updates the timestamp (should only be called by domain services)
        /// </summary>
        internal void UpdateTimestamp(DateTime timestamp)
        {
            UpdatedAt = timestamp;
        }

        /// <summary>
        /// Updates campaign details
        /// </summary>
        public void UpdateDetails(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            // Timestamp will be updated by service layer
        }

        /// <summary>
        /// Starts the campaign
        /// </summary>
        public void Start(DateTime startDate)
        {
            CampaignStateValidator.ValidateTransition(Status, CampaignStatus.Active);
            Status = CampaignStatus.Active;
            StartDate = startDate;
        }

        /// <summary>
        /// Stops the campaign
        /// </summary>
        public void Stop(DateTime endDate)
        {
            CampaignStateValidator.ValidateTransition(Status, CampaignStatus.Stopped);
            Status = CampaignStatus.Stopped;
            EndDate = endDate;
        }

        /// <summary>
        /// Completes the campaign
        /// </summary>
        public void Complete(DateTime endDate)
        {
            CampaignStateValidator.ValidateTransition(Status, CampaignStatus.Completed);
            Status = CampaignStatus.Completed;
            EndDate = endDate;
        }
    }

    /// <summary>
    /// Campaign status enumeration
    /// </summary>
    public enum CampaignStatus
    {
        Draft = 0,
        Active = 1,
        Stopped = 2,
        Completed = 3
    }
}