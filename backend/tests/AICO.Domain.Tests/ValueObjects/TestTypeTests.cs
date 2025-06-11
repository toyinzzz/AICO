using System;
using AICO.Domain.ValueObjects;
using Xunit;

namespace AICO.Domain.Tests.ValueObjects
{
    public class TestTypeTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldReturnTestType()
        {
            // Arrange
            var value = "Button Color";

            // Act
            var testType = TestType.Create(value);

            // Assert
            Assert.NotNull(testType);
            Assert.Equal(value, testType.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Create_WithInvalidValue_ShouldThrowArgumentException(string invalidValue)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => TestType.Create(invalidValue));
        }

        [Fact]
        public void Equals_WithSameValue_ShouldReturnTrue()
        {
            // Arrange
            var testType1 = TestType.Create("Button Color");
            var testType2 = TestType.Create("Button Color");

            // Act & Assert
            Assert.Equal(testType1, testType2);
            Assert.True(testType1 == testType2);
        }

        [Fact]
        public void Equals_WithDifferentValue_ShouldReturnFalse()
        {
            // Arrange
            var testType1 = TestType.Create("Button Color");
            var testType2 = TestType.Create("Text Content");

            // Act & Assert
            Assert.NotEqual(testType1, testType2);
            Assert.True(testType1 != testType2);
        }

        [Fact]
        public void GetHashCode_WithSameValue_ShouldReturnSameHashCode()
        {
            // Arrange
            var testType1 = TestType.Create("Button Color");
            var testType2 = TestType.Create("Button Color");

            // Act & Assert
            Assert.Equal(testType1.GetHashCode(), testType2.GetHashCode());
        }
    }
}