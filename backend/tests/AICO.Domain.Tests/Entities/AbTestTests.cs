using AICO.Domain.Entities;
using AICO.Domain.Events;
using AICO.Domain.ValueObjects;
using Xunit;

namespace AICO.Domain.Tests.Entities
{
    public class AbTestTests
    {
        private AbTest CreateValidAbTest()
        {
            var abTest = AbTest.Create(
                "Test A/B Test",
                "Test Description",
                Guid.NewGuid(),
                TestType.Create("Button Color"),
                ".cta-button",
                "Buy Now",
                "conversion_rate"
            );
            return abTest;
        }

        [Fact]
        public void Create_WithValidData_ShouldReturnAbTest()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var name = "Test A/B Test";
            var description = "Test Description";
            var testType = TestType.Create("Button Color");
            var primaryMetric = "conversion_rate";
            var targetSelector = ".cta-button";
            var originalContent = "Buy Now";

            // Act
            var abTest = AbTest.Create(name, description, campaignId, testType, targetSelector, originalContent, primaryMetric);

            // Assert
            Assert.NotNull(abTest);
            Assert.Equal(campaignId, abTest.CampaignId);
            Assert.Equal(name, abTest.Name);
            Assert.Equal(description, abTest.Description);
            Assert.Equal(testType, abTest.TestType);
            Assert.Equal(primaryMetric, abTest.PrimaryMetric);
            Assert.Equal(AbTestStatus.Draft, abTest.Status);
            Assert.Empty(abTest.Variants);
            Assert.Empty(abTest.Conversions);
            Assert.Empty(abTest.DomainEvents);
        }

        [Fact]
        public void AddVariant_ShouldAddVariantToList()
        {
            // Arrange
            var abTest = CreateValidAbTest();
            var variant = new AbTestVariant(abTest.Id, "Variant A", "Test content", 50m, null, true);

            // Act
            abTest.AddVariant(variant);

            // Assert
            Assert.Single(abTest.Variants);
            Assert.Contains(variant, abTest.Variants);
        }

        [Fact]
        public void RemoveVariant_ShouldRemoveVariantFromList()
        {
            // Arrange
            var abTest = CreateValidAbTest();
            var variant = new AbTestVariant(abTest.Id, "Variant A", "Test content", 50m, null, true);
            abTest.AddVariant(variant);

            // Act
            abTest.RemoveVariant(variant.Id);

            // Assert
            Assert.Empty(abTest.Variants);
        }

        [Fact]
        public void ClearVariants_ShouldClearVariantList()
        {
            // Arrange
            var abTest = CreateValidAbTest();
            var variant1 = new AbTestVariant(abTest.Id, "Variant A", "Test content", 50m, null, true);
            var variant2 = new AbTestVariant(abTest.Id, "Variant B", "Test content", 50m, null, false);
            abTest.AddVariant(variant1);
            abTest.AddVariant(variant2);

            // Act
            abTest.ClearVariants();

            // Assert
            Assert.Empty(abTest.Variants);
        }

        [Fact]
        public void AddConversion_ShouldAddConversionToList()
        {
            // Arrange
            var abTest = CreateValidAbTest();
            var conversion = Conversion.Create(abTest.Id, ConversionType.Create("click"), DateTime.UtcNow.ToString(), 100.0m);

            // Act
            abTest.AddConversion(conversion);

            // Assert
            Assert.Single(abTest.Conversions);
            Assert.Contains(conversion, abTest.Conversions);
        }

        [Theory]
        [InlineData(nameof(AbTest.Start), AbTestStatus.Draft, AbTestStatus.Running)]
        [InlineData(nameof(AbTest.Pause), AbTestStatus.Running, AbTestStatus.Paused)]
        [InlineData(nameof(AbTest.Stop), AbTestStatus.Running, AbTestStatus.Stopped)]
        [InlineData(nameof(AbTest.Complete), AbTestStatus.Running, AbTestStatus.Completed)]
        [InlineData(nameof(AbTest.Archive), AbTestStatus.Completed, AbTestStatus.Archived)]
        [InlineData(nameof(AbTest.Resume), AbTestStatus.Paused, AbTestStatus.Running)]
        [InlineData(nameof(AbTest.MarkReadyToStart), AbTestStatus.Draft, AbTestStatus.ReadyToStart)]
        public void StatusChangeMethods_ShouldRaiseAbTestStatusChangedEvent(string methodName, AbTestStatus initialStatus, AbTestStatus newStatus)
        {
            // Arrange
            var abTest = CreateValidAbTest();
            abTest.GetType().GetProperty("Status").SetValue(abTest, initialStatus);
            abTest.ClearDomainEvents();

            // Act
            abTest.GetType().GetMethod(methodName).Invoke(abTest, null);

            // Assert
            var domainEvent = abTest.DomainEvents.FirstOrDefault();
            Assert.NotNull(domainEvent);
            Assert.IsType<AbTestStatusChangedEvent>(domainEvent);

            var statusChangedEvent = (AbTestStatusChangedEvent)domainEvent;
            Assert.Equal(abTest.Id, statusChangedEvent.AbTestId);
            Assert.Equal(initialStatus, statusChangedEvent.OldStatus);
            Assert.Equal(newStatus, statusChangedEvent.NewStatus);
            Assert.Equal(newStatus, abTest.Status);
        }
    }
}