using AICO.Domain.Entities;
using AICO.Domain.ValueObjects;
using Moq;
using Xunit;

namespace AICO.Domain.Tests.Entities
{
    public class AbTestTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnAbTest()
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var name = "Test A/B Test";
            var description = "Test Description";
            var testType = TestType.Create("Button Color");
            var trafficSplit = 50;
            var successMetric = "conversion_rate";
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddDays(30);

            // Act
            var abTest = AbTest.Create(campaignId, name, description, testType, trafficSplit, successMetric, startDate, endDate);

            // Assert
            Assert.NotNull(abTest);
            Assert.Equal(campaignId, abTest.CampaignId);
            Assert.Equal(name, abTest.Name);
            Assert.Equal(description, abTest.Description);
            Assert.Equal(testType, abTest.TestType);
            Assert.Equal(trafficSplit, abTest.TrafficSplit);
            Assert.Equal(successMetric, abTest.SuccessMetric);
            Assert.Equal(startDate, abTest.StartDate);
            Assert.Equal(endDate, abTest.EndDate);
            Assert.Equal(AbTestStatus.Draft, abTest.Status);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(101)]
        public void Create_WithInvalidTrafficSplit_ShouldThrowArgumentException(int invalidTrafficSplit)
        {
            // Arrange
            var campaignId = Guid.NewGuid();
            var testType = TestType.Create("Button Color");
            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = DateTime.UtcNow.AddDays(30);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                AbTest.Create(campaignId, "Test", "Description", testType, invalidTrafficSplit, "conversion_rate", startDate, endDate));
        }

        [Fact]
        public void StartTest_WithValidState_ShouldUpdateStatusAndStartDate()
        {
            // Arrange
            var abTest = CreateValidAbTest();
            var mockValidator = new Mock<IAbTestStateValidationService>();
            mockValidator.Setup(x => x.CanTransitionTo(AbTestStatus.Draft, AbTestStatus.Running))
                        .Returns(true);

            // Act
            abTest.StartTest(mockValidator.Object);

            // Assert
            Assert.Equal(AbTestStatus.Running, abTest.Status);
            Assert.True(abTest.ActualStartDate.HasValue);
            Assert.True(abTest.ActualStartDate.Value <= DateTime.UtcNow);
        }

        [Fact]
        public void CompleteTest_WithValidState_ShouldUpdateStatusAndEndDate()
        {
            // Arrange
            var abTest = CreateValidAbTest();
            var mockValidator = new Mock<IAbTestStateValidationService>();
            mockValidator.Setup(x => x.CanTransitionTo(AbTestStatus.Running, AbTestStatus.Completed))
                        .Returns(true);

            // Start the test first
            abTest.GetType().GetProperty("Status")?.SetValue(abTest, AbTestStatus.Running);

            // Act
            abTest.CompleteTest(mockValidator.Object);

            // Assert
            Assert.Equal(AbTestStatus.Completed, abTest.Status);
            Assert.True(abTest.ActualEndDate.HasValue);
            Assert.True(abTest.ActualEndDate.Value <= DateTime.UtcNow);
        }

        private AbTest CreateValidAbTest()
        {
            return AbTest.Create(
                Guid.NewGuid(),
                "Test A/B Test",
                "Test Description",
                TestType.Create("Button Color"),
                50,
                "conversion_rate",
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(30)
            );
        }
    }
}