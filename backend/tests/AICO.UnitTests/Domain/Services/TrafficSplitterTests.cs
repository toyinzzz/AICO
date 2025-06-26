using AICO.Domain.Entities;
using AICO.Domain.Services;
using Xunit;

namespace AICO.UnitTests.Domain.Services
{
    public class TrafficSplitterTests
    {
        private readonly TrafficSplitter _trafficSplitter;

        public TrafficSplitterTests()
        {
            _trafficSplitter = new TrafficSplitter();
        }

        [Fact]
        public void AssignVariant_WithEvenSplit_ShouldDistributeEvenly()
        {
            // Arrange
            var variants = new List<Variant>
            {
                new Variant("Control", Guid.NewGuid(), "<html>Control</html>", 50m, true),
                new Variant("Variant A", Guid.NewGuid(), "<html>Variant A</html>", 50m)
            };
            var trafficSplit = 50; // 50/50 split
            var assignments = new Dictionary<Guid, int>();

            // Act - Simulate 1000 user assignments
            for (int i = 0; i < 1000; i++)
            {
                var userId = $"user_{i}";
                var assignedVariant = _trafficSplitter.AssignVariant(variants, userId);

                if (!assignments.ContainsKey(assignedVariant.Id))
                    assignments[assignedVariant.Id] = 0;
                assignments[assignedVariant.Id]++;
            }

            // Assert - Should be roughly 50/50 distribution (within 10% tolerance)
            Assert.Equal(2, assignments.Count);
            foreach (var count in assignments.Values)
            {
                Assert.True(count >= 400 && count <= 600, $"Distribution should be roughly even, got {count}");
            }
        }

        [Fact]
        public void AssignVariant_WithSameUserId_ShouldReturnConsistentVariant()
        {
            // Arrange
            var variants = new List<Variant>
            {
                new Variant("Control", Guid.NewGuid(), "<html>Control</html>", 50m, true),
                new Variant("Variant A", Guid.NewGuid(), "<html>Variant A</html>", 50m)
            };
            var userId = "consistent_user";
            var trafficSplit = 50;

            // Act - Assign same user multiple times
            var firstAssignment = _trafficSplitter.AssignVariant(variants, userId);
                var secondAssignment = _trafficSplitter.AssignVariant(variants, userId);
                var thirdAssignment = _trafficSplitter.AssignVariant(variants, userId);

            // Assert - Should always get the same variant
            Assert.Equal(firstAssignment.Id, secondAssignment.Id);
            Assert.Equal(firstAssignment.Id, thirdAssignment.Id);
        }

        [Theory]
        [InlineData(10)] // 10% to variant, 90% to control
        [InlineData(25)] // 25% to variant, 75% to control
        [InlineData(75)] // 75% to variant, 25% to control
        [InlineData(90)] // 90% to variant, 10% to control
        public void AssignVariant_WithCustomSplit_ShouldRespectTrafficAllocation(int variantPercentage)
        {
            // Arrange
            var controlVariant = new Variant("Control", Guid.NewGuid(), "<html>Control</html>", 50m, true);
            var testVariant = new Variant("Test", Guid.NewGuid(), "<html>Test</html>", 50m);
            var variants = new List<Variant> { controlVariant, testVariant };

            var assignments = new Dictionary<Guid, int>();
            var totalAssignments = 10000;

            // Act
            for (int i = 0; i < totalAssignments; i++)
            {
                var userId = $"user_{i}";
                var assignedVariant = _trafficSplitter.AssignVariant(variants, userId);

                if (!assignments.ContainsKey(assignedVariant.Id))
                    assignments[assignedVariant.Id] = 0;
                assignments[assignedVariant.Id]++;
            }

            // Assert - Check distribution is within 2% tolerance
            var testVariantCount = assignments.GetValueOrDefault(testVariant.Id, 0);
            var actualPercentage = (double)testVariantCount / totalAssignments * 100;

            Assert.True(Math.Abs(actualPercentage - variantPercentage) <= 2,
                $"Expected {variantPercentage}% but got {actualPercentage:F1}%");
        }

        [Fact]
        public void AssignVariant_WithMultipleVariants_ShouldDistributeCorrectly()
        {
            // Arrange
            var variants = new List<Variant>
            {
                new Variant("Control", Guid.NewGuid(), "<html>Control</html>", 25m, true),
                new Variant("Variant A", Guid.NewGuid(), "<html>Variant A</html>", 25m),
                new Variant("Variant B", Guid.NewGuid(), "<html>Variant B</html>", 25m),
                new Variant("Variant C", Guid.NewGuid(), "<html>Variant C</html>", 25m)
            };
            var trafficSplit = 25; // Equal 25% split among 4 variants
            var assignments = new Dictionary<Guid, int>();
            var totalAssignments = 10000;

            // Act
            for (int i = 0; i < totalAssignments; i++)
            {
                var userId = $"user_{i}";
                var assignedVariant = _trafficSplitter.AssignVariant(variants, userId);

                if (!assignments.ContainsKey(assignedVariant.Id))
                    assignments[assignedVariant.Id] = 0;
                assignments[assignedVariant.Id]++;
            }

            // Assert - Each variant should get roughly 25% (within 2% tolerance)
            Assert.Equal(4, assignments.Count);
            foreach (var count in assignments.Values)
            {
                var percentage = (double)count / totalAssignments * 100;
                Assert.True(percentage >= 23 && percentage <= 27,
                    $"Each variant should get ~25%, got {percentage:F1}%");
            }
        }
    }
}