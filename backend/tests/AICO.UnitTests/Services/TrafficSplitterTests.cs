using AICO.Domain.Entities;
using Xunit;

namespace AICO.UnitTests.Services
{
    public class TrafficSplitterTests
    {
        private readonly TrafficSplitter _trafficSplitter;

        public TrafficSplitterTests()
        {
            _trafficSplitter = new TrafficSplitter();
        }

        [Fact]
        public void AssignVariant_WithEqualSplit_ShouldDistributeEvenly()
        {
            // Arrange
            var variants = new List<Variant>
            {
                Variant.Create(Guid.NewGuid(), "Control", "Original", 50, ".btn {}", true),
                Variant.Create(Guid.NewGuid(), "Variant A", "Modified", 50, ".btn { color: red; }", false)
            };

            var assignments = new Dictionary<Guid, int>();
            var totalAssignments = 10000;

            // Act
            for (int i = 0; i < totalAssignments; i++)
            {
                var userId = Guid.NewGuid().ToString();
                var assignedVariant = _trafficSplitter.AssignVariant(userId, variants);

                if (!assignments.ContainsKey(assignedVariant.Id))
                    assignments[assignedVariant.Id] = 0;
                assignments[assignedVariant.Id]++;
            }

            // Assert
            Assert.Equal(2, assignments.Count);
            foreach (var assignment in assignments)
            {
                var percentage = (double)assignment.Value / totalAssignments * 100;
                Assert.True(percentage >= 45 && percentage <= 55, $"Expected ~50%, got {percentage:F1}%");
            }
        }

        [Fact]
        public void AssignVariant_WithSameUserId_ShouldReturnConsistentVariant()
        {
            // Arrange
            var userId = "user123";
            var variants = new List<Variant>
            {
                Variant.Create(Guid.NewGuid(), "Control", "Original", 50, ".btn {}", true),
                Variant.Create(Guid.NewGuid(), "Variant A", "Modified", 50, ".btn { color: red; }", false)
            };

            // Act
            var firstAssignment = _trafficSplitter.AssignVariant(userId, variants);
            var secondAssignment = _trafficSplitter.AssignVariant(userId, variants);
            var thirdAssignment = _trafficSplitter.AssignVariant(userId, variants);

            // Assert
            Assert.Equal(firstAssignment.Id, secondAssignment.Id);
            Assert.Equal(firstAssignment.Id, thirdAssignment.Id);
        }

        [Theory]
        [InlineData(70, 30)]
        [InlineData(80, 20)]
        [InlineData(90, 10)]
        public void AssignVariant_WithCustomSplit_ShouldRespectPercentages(int controlPercentage, int variantPercentage)
        {
            // Arrange
            var variants = new List<Variant>
            {
                Variant.Create(Guid.NewGuid(), "Control", "Original", controlPercentage, ".btn {}", true),
                Variant.Create(Guid.NewGuid(), "Variant A", "Modified", variantPercentage, ".btn { color: red; }", false)
            };

            var assignments = new Dictionary<Guid, int>();
            var totalAssignments = 10000;

            // Act
            for (int i = 0; i < totalAssignments; i++)
            {
                var userId = Guid.NewGuid().ToString();
                var assignedVariant = _trafficSplitter.AssignVariant(userId, variants);

                if (!assignments.ContainsKey(assignedVariant.Id))
                    assignments[assignedVariant.Id] = 0;
                assignments[assignedVariant.Id]++;
            }

            // Assert
            var controlVariant = variants.First(v => v.IsControl);
            var testVariant = variants.First(v => !v.IsControl);

            var controlActual = (double)assignments[controlVariant.Id] / totalAssignments * 100;
            var variantActual = (double)assignments[testVariant.Id] / totalAssignments * 100;

            Assert.True(Math.Abs(controlActual - controlPercentage) < 5,
                $"Control: Expected {controlPercentage}%, got {controlActual:F1}%");
            Assert.True(Math.Abs(variantActual - variantPercentage) < 5,
                $"Variant: Expected {variantPercentage}%, got {variantActual:F1}%");
        }
    }
}