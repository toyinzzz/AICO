using System;
using AICO.Domain.Entities;
using Xunit;

namespace AICO.Domain.Tests.Entities
{
    public class SnippetTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnSnippet()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            var name = "Header Tracking";
            var description = "Analytics tracking code";
            var content = "<script>console.log('tracking');</script>";
            var placement = "head";
            var isActive = true;

            // Act
            var snippet = Snippet.Create(websiteId, name, description, content, placement, isActive);

            // Assert
            Assert.NotNull(snippet);
            Assert.Equal(websiteId, snippet.WebsiteId);
            Assert.Equal(name, snippet.Name);
            Assert.Equal(description, snippet.Description);
            Assert.Equal(content, snippet.Content);
            Assert.Equal(placement, snippet.Placement);
            Assert.Equal(isActive, snippet.IsActive);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var websiteId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Snippet.Create(websiteId, invalidName, "Description", "<script></script>", "head", true));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_WithInvalidContent_ShouldThrowArgumentException(string invalidContent)
        {
            // Arrange
            var websiteId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Snippet.Create(websiteId, "Test", "Description", invalidContent, "head", true));
        }

        [Fact]
        public void Activate_ShouldSetIsActiveToTrue()
        {
            // Arrange
            var snippet = Snippet.Create(Guid.NewGuid(), "Test", "Description", "<script></script>", "head", false);

            // Act
            snippet.Activate();

            // Assert
            Assert.True(snippet.IsActive);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var snippet = Snippet.Create(Guid.NewGuid(), "Test", "Description", "<script></script>", "head", true);

            // Act
            snippet.Deactivate();

            // Assert
            Assert.False(snippet.IsActive);
        }
    }
}