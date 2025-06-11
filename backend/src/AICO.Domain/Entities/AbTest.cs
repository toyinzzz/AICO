using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AICO.Domain.ValueObjects;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents an A/B test within a campaign
    /// </summary>
    public class AbTest : BaseEntity
    {
        /// <summary>
        /// Test name
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; private set; }

        /// <summary>
        /// Test description
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; private set; }

        /// <summary>
        /// Campaign this test belongs to
        /// </summary>
        [Required]
        public Guid CampaignId { get; private set; }
        public Campaign Campaign { get; private set; }

        /// <summary>
        /// Test status
        /// </summary>
        [Required]
        public AbTestStatus Status { get; private set; }

        /// <summary>
        /// Test type (using value object for type safety)
        /// </summary>
        [Required]
        public TestType TestType { get; private set; }

        /// <summary>
        /// Target element selector (CSS selector)
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string TargetSelector { get; private set; }

        /// <summary>
        /// Original content (control)
        /// </summary>
        [Required]
        public string OriginalContent { get; private set; }

        /// <summary>
        /// Test variants
        /// </summary>
        public ICollection<Variant> Variants { get; private set; } = new List<Variant>();

        /// <summary>
        /// Conversions tracked for this test
        /// </summary>
        public ICollection<Conversion> Conversions { get; private set; } = new List<Conversion>();

        /// <summary>
        /// Private constructor for EF Core
        /// </summary>
        private AbTest() { }

        /// <summary>
        /// Creates a new A/B test
        /// </summary>
        public AbTest(string name, string description, Guid campaignId, TestType testType, 
                     string targetSelector, string originalContent)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            CampaignId = campaignId;
            TestType = testType ?? throw new ArgumentNullException(nameof(testType));
            TargetSelector = targetSelector ?? throw new ArgumentNullException(nameof(targetSelector));
            OriginalContent = originalContent ?? throw new ArgumentNullException(nameof(originalContent));
            Status = AbTestStatus.Draft;
        }

        /// <summary>
        /// Updates the timestamp (should only be called by domain services)
        /// </summary>
        internal void UpdateTimestamp(DateTime timestamp)
        {
            UpdatedAt = timestamp;
        }

        /// <summary>
        /// Starts the A/B test
        /// </summary>
        public void Start()
        {
            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.Running);
            Status = AbTestStatus.Running;
            // Removed DateTime.UtcNow - will be handled by service layer
        }

        /// <summary>
        /// Stops the A/B test
        /// </summary>
        public void Stop()
        {
            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.Stopped);
            Status = AbTestStatus.Stopped;
            // Removed DateTime.UtcNow - will be handled by service layer
        }

        /// <summary>
        /// Completes the A/B test
        /// </summary>
        public void Complete()
        {
            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.Completed);
            Status = AbTestStatus.Completed;
        }
    }

    /// <summary>
    /// A/B test status enumeration
    /// </summary>
    public enum AbTestStatus
    {
        Draft = 0,
        Running = 1,
        Stopped = 2,
        Completed = 3
    }
}