using AICO.Domain.ValueObjects;
using AICO.Domain.Validators;
using AICO.Domain.Events;
using AICO.Domain.Interfaces.Events;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents an A/B test within a campaign with comprehensive testing capabilities
    /// </summary>
    public class AbTest : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Test name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Test description
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Test hypothesis - what we expect to happen
        /// </summary>
        public string? Hypothesis { get; private set; }

        /// <summary>
        /// Expected outcome description
        /// </summary>
        public string? ExpectedOutcome { get; private set; }

        /// <summary>
        /// Campaign this test belongs to
        /// </summary>
        public Guid CampaignId { get; private set; }
        public Campaign Campaign { get; private set; }

        /// <summary>
        /// Test status
        /// </summary>
        public AbTestStatus Status { get; private set; }

        /// <summary>
        /// Test type (using value object for type safety)
        /// </summary>
        public TestType TestType { get; private set; }

        /// <summary>
        /// Website this test belongs to
        /// </summary>
        public Guid WebsiteId { get; private set; }
        public Website Website { get; private set; }

        /// <summary>
        /// Target element selector (CSS selector)
        /// </summary>
        public string TargetSelector { get; private set; }

        /// <summary>
        /// Original content (control)
        /// </summary>
        public string OriginalContent { get; private set; }

        /// <summary>
        /// Primary metric being measured (e.g., "conversion_rate", "click_through_rate")
        /// </summary>
        public string PrimaryMetric { get; private set; }

        /// <summary>
        /// Percentage of traffic to include in test (0-100)
        /// </summary>
        public decimal TrafficPercentage { get; private set; }

        /// <summary>
        /// Minimum sample size required for statistical significance
        /// </summary>
        public int MinimumSampleSize { get; private set; }

        /// <summary>
        /// Statistical significance level (e.g., 0.05 for 95% confidence)
        /// </summary>
        public decimal SignificanceLevel { get; private set; }

        /// <summary>
        /// Statistical power (e.g., 0.8 for 80% power)
        /// </summary>
        public decimal StatisticalPower { get; private set; }

        /// <summary>
        /// Baseline conversion rate (if known)
        /// </summary>
        public decimal? BaselineConversionRate { get; private set; }

        /// <summary>
        /// Expected lift/improvement (as decimal, e.g., 0.1 for 10% improvement)
        /// </summary>
        public decimal? ExpectedLift { get; private set; }

        /// <summary>
        /// Minimum duration in days
        /// </summary>
        public int MinimumDurationDays { get; private set; }

        /// <summary>
        /// Planned end date
        /// </summary>
        public DateTime? PlannedEndDate { get; private set; }

        /// <summary>
        /// When the test was actually started
        /// </summary>
        public DateTime? StartedAt { get; private set; }

        /// <summary>
        /// When the test was ended
        /// </summary>
        public DateTime? EndedAt { get; private set; }

        /// <summary>
        /// When the test was paused (if applicable)
        /// </summary>
        public DateTime? PausedAt { get; private set; }

        /// <summary>
        /// Audience filter criteria (JSON or specific format)
        /// </summary>
        public string? AudienceFilter { get; private set; }

        /// <summary>
        /// Whether this test affects all users or a specific segment
        /// </summary>
        public bool IsGlobalTest { get; private set; }

        /// <summary>
        /// Whether the test is currently enabled
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Priority for handling overlapping tests
        /// </summary>
        public int Priority { get; private set; }

        private readonly List<AbTestVariant> _variants = new();
        public IReadOnlyCollection<AbTestVariant> Variants => _variants.AsReadOnly();

        private readonly List<Conversion> _conversions = new();
        public IReadOnlyCollection<Conversion> Conversions => _conversions.AsReadOnly();

        private readonly List<IDomainEvent> _domainEvents = new();
        public new IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public new void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor for EF Core
        /// </summary>
        public AbTest() 
        {
            Name = string.Empty;
            Campaign = null!; // EF Core will populate this
            TestType = null!; // EF Core will populate this, assuming it's a reference type
            Website = null!;  // EF Core will populate this
            TargetSelector = string.Empty;
            OriginalContent = string.Empty;
            PrimaryMetric = string.Empty;
            // Initialize other non-nullable properties if necessary, though the warnings focused on these.
            // Collections are already initialized: Variants and Conversions.
        }



        /// <summary>
        /// Creates a new A/B test with essential parameters
        /// </summary>
        public AbTest(string name, string? description, Guid campaignId, TestType testType,
                     string targetSelector, string originalContent, string primaryMetric)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            CampaignId = campaignId;
            TestType = testType ?? throw new ArgumentNullException(nameof(testType));
            TargetSelector = targetSelector ?? throw new ArgumentNullException(nameof(targetSelector));
            OriginalContent = originalContent ?? throw new ArgumentNullException(nameof(originalContent));
            PrimaryMetric = primaryMetric ?? throw new ArgumentNullException(nameof(primaryMetric));
            
            // Set defaults
            Status = AbTestStatus.Draft;
            TrafficPercentage = 50m;
            MinimumSampleSize = 1000;
            SignificanceLevel = 0.05m;
            StatisticalPower = 0.8m;
            MinimumDurationDays = 7;
            IsGlobalTest = true;
            IsEnabled = true;
            Priority = 5;
        }

        /// <summary>
        /// Creates a new A/B test with full configuration
        /// </summary>
        public AbTest(string name, string? description, string? hypothesis, string? expectedOutcome,
                     Guid campaignId, TestType testType, string targetSelector, string originalContent,
                     string primaryMetric, decimal trafficPercentage, int minimumSampleSize,
                     decimal significanceLevel, decimal statisticalPower, int minimumDurationDays,
                     DateTime? plannedEndDate = null, string? audienceFilter = null,
                     bool isGlobalTest = true, int priority = 5)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Hypothesis = hypothesis;
            ExpectedOutcome = expectedOutcome;
            CampaignId = campaignId;
            TestType = testType ?? throw new ArgumentNullException(nameof(testType));
            TargetSelector = targetSelector ?? throw new ArgumentNullException(nameof(targetSelector));
            OriginalContent = originalContent ?? throw new ArgumentNullException(nameof(originalContent));
            PrimaryMetric = primaryMetric ?? throw new ArgumentNullException(nameof(primaryMetric));
            
            TrafficPercentage = ValidateRange(trafficPercentage, 0.01m, 100m, nameof(trafficPercentage));
            MinimumSampleSize = ValidateRange(minimumSampleSize, 1, int.MaxValue, nameof(minimumSampleSize));
            SignificanceLevel = ValidateRange(significanceLevel, 0.01m, 0.1m, nameof(significanceLevel));
            StatisticalPower = ValidateRange(statisticalPower, 0.7m, 0.95m, nameof(statisticalPower));
            MinimumDurationDays = ValidateRange(minimumDurationDays, 1, 365, nameof(minimumDurationDays));
            Priority = ValidateRange(priority, 1, 10, nameof(priority));
            
            PlannedEndDate = plannedEndDate;
            AudienceFilter = audienceFilter;
            IsGlobalTest = isGlobalTest;
            
            Status = AbTestStatus.Draft;
            IsEnabled = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets baseline metrics for the test
        /// </summary>
        public void SetBaselineMetrics(decimal? baselineConversionRate, decimal? expectedLift)
        {
            if (Status != AbTestStatus.Draft)
                throw new InvalidOperationException("Cannot modify baseline metrics after test setup is complete");

            BaselineConversionRate = baselineConversionRate;
            ExpectedLift = expectedLift;
        }

        /// <summary>
        /// Updates test configuration (only allowed in Draft status)
        /// </summary>
        public void UpdateConfiguration(decimal trafficPercentage, int minimumSampleSize,
                                      decimal significanceLevel, decimal statisticalPower,
                                      int minimumDurationDays, DateTime? plannedEndDate = null)
        {
            if (Status != AbTestStatus.Draft)
                throw new InvalidOperationException("Cannot modify configuration after test setup is complete");

            TrafficPercentage = ValidateRange(trafficPercentage, 0.01m, 100m, nameof(trafficPercentage));
            MinimumSampleSize = ValidateRange(minimumSampleSize, 1, int.MaxValue, nameof(minimumSampleSize));
            SignificanceLevel = ValidateRange(significanceLevel, 0.01m, 0.1m, nameof(significanceLevel));
            StatisticalPower = ValidateRange(statisticalPower, 0.7m, 0.95m, nameof(statisticalPower));
            MinimumDurationDays = ValidateRange(minimumDurationDays, 1, 365, nameof(minimumDurationDays));
            PlannedEndDate = plannedEndDate;
        }

        /// <summary>
        /// Adds a variant to the test
        /// </summary>
        public void AddVariant(AbTestVariant variant)
        {
            if (Status != AbTestStatus.Draft)
                throw new InvalidOperationException("Cannot modify variants after test starts");

            if (_variants.Count >= 10)
                throw new InvalidOperationException("Maximum 10 variants allowed per test");

            if (variant == null)
                throw new ArgumentNullException(nameof(variant));

            _variants.Add(variant);
        }

        /// <summary>
        /// Removes a variant from the test
        /// </summary>
        public void RemoveVariant(Guid variantId)
        {
            if (Status != AbTestStatus.Draft)
                throw new InvalidOperationException("Cannot modify variants after test starts");

            var variant = _variants.FirstOrDefault(v => v.Id == variantId);
            if (variant != null)
            {
                _variants.Remove(variant);
            }
        }

        public void ClearVariants()
        {
            if (Status != AbTestStatus.Draft)
                throw new InvalidOperationException("Cannot modify variants after test starts");
            _variants.Clear();
        }

        public void AddConversion(Conversion conversion)
        {
            if (conversion == null) throw new ArgumentNullException(nameof(conversion));
            // Additional validation can be added here
            _conversions.Add(conversion);
        }

        /// <summary>
        /// Checks if the test can be started
        /// </summary>
        public bool CanStart()
        {
            return Status == AbTestStatus.Draft &&
                   Variants.Any() &&
                   !string.IsNullOrEmpty(PrimaryMetric) &&
                   MinimumSampleSize > 0 &&
                   IsEnabled;
        }

        /// <summary>
        /// Marks the test as ready to start
        /// </summary>
        public void MarkReadyToStart()
        {
            if (!CanStart())
                throw new InvalidOperationException("Test cannot be marked ready - missing requirements");

            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.ReadyToStart);
            var oldStatus = Status;
            Status = AbTestStatus.ReadyToStart;
            AddDomainEvent(new AbTestStatusChangedEvent(this.Id, oldStatus, Status));
        }

        /// <summary>
        /// Starts the A/B test
        /// </summary>
        public void Start()
        {
            if (Status == AbTestStatus.Draft && !CanStart())
                throw new InvalidOperationException("Test cannot be started - missing requirements");

            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.Running);
            var oldStatus = Status;
            Status = AbTestStatus.Running;
            StartedAt = DateTime.UtcNow;
            AddDomainEvent(new AbTestStatusChangedEvent(this.Id, oldStatus, Status));
        }

        /// <summary>
        /// Pauses the A/B test
        /// </summary>
        public void Pause()
        {
            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.Paused);
            var oldStatus = Status;
            Status = AbTestStatus.Paused;
            PausedAt = DateTime.UtcNow;
            AddDomainEvent(new AbTestStatusChangedEvent(this.Id, oldStatus, Status));
        }

        /// <summary>
        /// Resumes the A/B test from paused state
        /// </summary>
        public void Resume()
        {
            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.Running);
            var oldStatus = Status;
            Status = AbTestStatus.Running;
            PausedAt = null;
            AddDomainEvent(new AbTestStatusChangedEvent(this.Id, oldStatus, Status));
        }

        /// <summary>
        /// Stops the A/B test
        /// </summary>
        public void Stop()
        {
            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.Stopped);
            var oldStatus = Status;
            Status = AbTestStatus.Stopped;
            EndedAt = DateTime.UtcNow;
            AddDomainEvent(new AbTestStatusChangedEvent(this.Id, oldStatus, Status));
        }

        /// <summary>
        /// Completes the A/B test
        /// </summary>
        public void Complete()
        {
            AbTestStateValidator.ValidateTransition(Status, AbTestStatus.Completed);
            var oldStatus = Status;
            Status = AbTestStatus.Completed;
            EndedAt = DateTime.UtcNow;
            AddDomainEvent(new AbTestStatusChangedEvent(this.Id, oldStatus, Status));
        }

        /// <summary>
        /// Archives the test
        /// </summary>
        public void Archive()
        {
            if (Status != AbTestStatus.Completed && Status != AbTestStatus.Stopped)
                throw new InvalidOperationException("Can only archive completed or stopped tests");

            var oldStatus = Status;
            Status = AbTestStatus.Archived;
            AddDomainEvent(new AbTestStatusChangedEvent(this.Id, oldStatus, Status));
        }

        /// <summary>
        /// Enables or disables the test
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        /// <summary>
        /// Updates the test priority
        /// </summary>
        public void SetPriority(int priority)
        {
            Priority = ValidateRange(priority, 1, 10, nameof(priority));
        }

        /// <summary>
        /// Checks if the test has been running for the minimum duration
        /// </summary>
        public bool HasRunMinimumDuration()
        {
            if (StartedAt == null) return false;
            return DateTime.UtcNow.Subtract(StartedAt.Value).TotalDays >= MinimumDurationDays;
        }

        /// <summary>
        /// Checks if the test has reached minimum sample size
        /// </summary>
        public bool HasMinimumSampleSize()
        {
            var totalSamples = Variants.Sum(v => v.Views);
            return totalSamples >= MinimumSampleSize;
        }

        /// <summary>
        /// Checks if the test can be completed (has minimum duration and sample size)
        /// </summary>
        public bool CanComplete()
        {
            return Status == AbTestStatus.Running &&
                   HasRunMinimumDuration() &&
                   HasMinimumSampleSize();
        }

        /// <summary>
        /// Gets the winning variant (if test is completed and has statistical significance)
        /// </summary>
        public AbTestVariant? GetWinningVariant()
        {
            if (Status != AbTestStatus.Completed)
                throw new InvalidOperationException("Test must be completed to determine winner");

            // This would typically delegate to a domain service for statistical analysis
            // For now, return the variant with the highest conversion rate
            return Variants
                .Where(v => v.Conversions > 0)
                .OrderByDescending(v => v.GetConversionRate())
                .FirstOrDefault();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the timestamp (should only be called by domain services)
        /// </summary>
        internal void UpdateTimestamp(DateTime timestamp)
        {
            UpdatedAt = timestamp;
        }

        #endregion

        #region Private Methods

        private T ValidateRange<T>(T value, T min, T max, string paramName) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(paramName, $"Value must be between {min} and {max}");
            return value;
        }

        private int GetControlImpressions()
        {
            // This would be implemented based on how you track control group impressions
            // For now, return 0 as placeholder
            return 0;
        }

        

        public static AbTest Create(string name, string? description, Guid campaignId, TestType testType,
                                    string targetSelector, string originalContent, string primaryMetric)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A/B test name cannot be empty.", nameof(name));
            }
            if (campaignId == Guid.Empty)
            {
                throw new ArgumentException("CampaignId cannot be empty.", nameof(campaignId));
            }
            if (string.IsNullOrWhiteSpace(targetSelector))
            {
                throw new ArgumentException("Target selector cannot be empty.", nameof(targetSelector));
            }
            if (string.IsNullOrWhiteSpace(primaryMetric))
            {
                throw new ArgumentException("Primary metric cannot be empty.", nameof(primaryMetric));
            }

            var abTest = new AbTest
            {
                Name = name,
                Description = description,
                CampaignId = campaignId,
                TestType = testType,
                TargetSelector = targetSelector,
                OriginalContent = originalContent,
                PrimaryMetric = primaryMetric,
                Status = AbTestStatus.Draft,
                TrafficPercentage = 50m,
                MinimumSampleSize = 1000,
                SignificanceLevel = 0.05m,
                StatisticalPower = 0.8m,
                MinimumDurationDays = 7,
                IsGlobalTest = true,
                IsEnabled = true,
                Priority = 5
            };

            return abTest;
        }

        #endregion
    }

    /// <summary>
    /// A/B test status enumeration
    /// </summary>
    public enum AbTestStatus
    {
        Draft = 0,
        ReadyToStart = 1,
        Running = 2,
        Paused = 3,
        Stopped = 4,
        Completed = 5,
        Archived = 6
    }
}