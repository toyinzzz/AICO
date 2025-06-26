using AICO.Application.Interfaces.ExternalServices;
using AICO.Domain.DTOs;
using AICO.Domain.Services;
using Moq;
using Xunit;

namespace AICO.UnitTests.Services
{
    public class AIVariantGeneratorTests
    {
        private readonly Mock<IAIService> _mockAIService;
        private readonly AIVariantGenerator _variantGenerator;

        public AIVariantGeneratorTests()
        {
            _mockAIService = new Mock<IAIService>();
            _variantGenerator = new AIVariantGenerator(_mockAIService.Object);
        }

        [Fact]
        public async Task GenerateVariants_WithValidInput_ShouldReturnVariants()
        {
            // Arrange
            var originalContent = "<button class='btn'>Buy Now</button>";
            var variantCount = 3;
            var testGoal = "Increase click-through rate";

            var expectedVariants = new List<AIGeneratedVariant>
            {
                new AIGeneratedVariant
                {
                    Content = "<button class='btn btn-primary'>Purchase Now</button>",
                    Description = "More action-oriented CTA",
                    ConfidenceScore = 0.85m
                },
                new AIGeneratedVariant
                {
                    Content = "<button class='btn btn-success'>Get Started</button>",
                    Description = "Softer approach",
                    ConfidenceScore = 0.78m
                }
            };

            _mockAIService.Setup(s => s.GenerateVariantsAsync(originalContent, variantCount, testGoal))
                .ReturnsAsync(expectedVariants);

            // Act
            var result = await _variantGenerator.GenerateVariantsAsync(originalContent, variantCount, testGoal);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, v => Assert.True(v.ConfidenceScore > 0.7m));
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