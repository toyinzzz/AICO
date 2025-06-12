using AICO.Domain.Entities;
using Xunit;

namespace AICO.Domain.Tests.Entities
{
    public class VariantTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnVariant()
        {
            // Arrange
            var abTestId = Guid.NewGuid();
            var name = "Control Variant";
            var description = "Original button design";
            var trafficPercentage = 50;
            var cssChanges = ".button { background-color: blue; }";
            var isControl = true;

            // Act
            var variant = Variant.Create(abTestId, name, description, trafficPercentage, cssChanges, isControl);

            // Assert
            Assert.NotNull(variant);
            Assert.Equal(abTestId, variant.AbTestId);
            Assert.Equal(name, variant.Name);
            Assert.Equal(description, variant.Description);
            Assert.Equal(trafficPercentage, variant.TrafficPercentage);
            Assert.Equal(cssChanges, variant.CssChanges);
            Assert.Equal(isControl, variant.IsControl);
            Assert.True(variant.IsActive);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var abTestId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                Variant.Create(abTestId, invalidName, "Description", 50, ".button {}", false));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(101)]
        public void Create_WithInvalidTrafficPercentage_ShouldThrowArgumentException(int invalidPercentage)
        {
            // Arrange
            var abTestId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                Variant.Create(abTestId, "Test Variant", "Description", invalidPercentage, ".button {}", false));
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var variant = Variant.Create(Guid.NewGuid(), "Test", "Description", 50, ".button {}", false);

            // Act
            variant.Deactivate();

            // Assert
            Assert.False(variant.IsActive);
        }

        [Fact]
        public void UpdateTrafficPercentage_WithValidPercentage_ShouldUpdateValue()
        {
            // Arrange
            var variant = Variant.Create(Guid.NewGuid(), "Test", "Description", 50, ".button {}", false);
            var newPercentage = 75;

            // Act
            variant.UpdateTrafficPercentage(newPercentage);

            // Assert
            Assert.Equal(newPercentage, variant.TrafficPercentage);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void UpdateTrafficPercentage_WithInvalidPercentage_ShouldThrowArgumentException(int invalidPercentage)
        {
            // Arrange
            var variant = Variant.Create(Guid.NewGuid(), "Test", "Description", 50, ".button {}", false);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => variant.UpdateTrafficPercentage(invalidPercentage));
        }
    }
}