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
            // Snippet constructor: Guid websiteId, string name, string code, string type, string version = "1.0.0", string configuration = null
            var snippet = new Snippet(websiteId, name, content, placement); // description and isActive are not in constructor
            
            // Set IsActive if the test intended a specific state different from default
            if (!isActive) snippet.Deactivate(); // Default is true, so only deactivate if test expects false

            // Assert
            Assert.NotNull(snippet);
            Assert.Equal(websiteId, snippet.WebsiteId);
            Assert.Equal(name, snippet.Name);
            // Assert.Equal(description, snippet.Description); // Description is not a direct property set by this constructor
            Assert.Equal(content, snippet.Code); // Property is Code, not Content
            Assert.Equal(placement, snippet.Type); // Property is Type, not Placement
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
            Assert.Throws<ArgumentNullException>(() => // Constructor throws ArgumentNullException for null name/code/type
                new Snippet(websiteId, invalidName, "<script></script>", "head"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_WithInvalidContent_ShouldThrowArgumentException(string invalidContent)
        {
            // Arrange
            var websiteId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => // Constructor throws ArgumentNullException for null name/code/type
                new Snippet(websiteId, "Test", invalidContent, "head"));
        }

        [Fact]
        public void Activate_ShouldSetIsActiveToTrue()
        {
            // Arrange
            var snippet = new Snippet(Guid.NewGuid(), "Test", "<script></script>", "head");
            snippet.Deactivate(); // Ensure it starts as false for this test

            // Act
            snippet.Activate();

            // Assert
            Assert.True(snippet.IsActive);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            // Arrange
            var snippet = new Snippet(Guid.NewGuid(), "Test", "<script></script>", "head");
            // Default is active, so no change needed here if test expects true initially

            // Act
            snippet.Deactivate();

            // Assert
            Assert.False(snippet.IsActive);
        }
    }
}