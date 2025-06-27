using AICO.Domain.Services;
using DomainIAIService = AICO.Domain.Interfaces.ExternalServices.IAIService;
using Moq;
using Xunit;

namespace AICO.UnitTests.Services
{
    public class AIVariantGeneratorTests
    {
        private readonly Mock<DomainIAIService> _mockAIService;
        private readonly AIVariantGenerator _variantGenerator;

        public AIVariantGeneratorTests()
        {
            _mockAIService = new Mock<DomainIAIService>();
            _variantGenerator = new AIVariantGenerator(_mockAIService.Object);
        }

        [Fact]
        public async Task GenerateVariants_WithValidInput_ShouldReturnVariants()
        {
            // Arrange
            var originalContent = "<button class='btn'>Buy Now</button>";
            var variantCount = 3;
            var testGoal = "Increase click-through rate";

            // Expected content variations from AI service
            var expectedContentVariations = new List<string>
            {
                "<button class='btn btn-primary'>Purchase Now</button>",
                "<button class='btn btn-success'>Get Started</button>"
            };

            _mockAIService.Setup(s => s.GenerateContentVariationsAsync(originalContent, variantCount))
                .ReturnsAsync(expectedContentVariations);

            // Act
            var result = await _variantGenerator.GenerateVariants(originalContent, variantCount, Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, v => Assert.False(string.IsNullOrEmpty(v.Content)));
        }



        [Fact]
        public async Task OptimizeContent_WithTargetAudience_ShouldReturnOptimizedContent()
        {
            // Arrange
            var content = "Buy now for great deals!";
            var targetAudience = "young professionals";

            // Act
            var optimizedContent = await _variantGenerator.OptimizeContent(content, targetAudience);

            // Assert
            Assert.NotNull(optimizedContent);
            Assert.NotEmpty(optimizedContent);
        }
    }
}