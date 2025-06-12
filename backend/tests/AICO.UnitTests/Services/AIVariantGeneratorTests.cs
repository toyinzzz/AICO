using AICO.Application.Interfaces.ExternalServices;
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
            var brandGuidelines = new BrandGuidelines
            {
                PrimaryColor = "#007bff",
                SecondaryColor = "#6c757d",
                FontFamily = "Arial, sans-serif",
                ToneOfVoice = "Professional"
            };

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

            _mockAIService.Setup(s => s.GenerateVariantsAsync(originalContent, brandGuidelines))
                .ReturnsAsync(expectedVariants);

            // Act
            var result = await _variantGenerator.GenerateVariantsAsync(originalContent, brandGuidelines);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, v => Assert.True(v.ConfidenceScore > 0.7m));
        }

        [Fact]
        public async Task ValidateBrandConsistency_WithValidVariant_ShouldReturnTrue()
        {
            // Arrange
            var variant = "<button style='color: #007bff; font-family: Arial;'>Buy Now</button>";
            var brandGuidelines = new BrandGuidelines
            {
                PrimaryColor = "#007bff",
                FontFamily = "Arial, sans-serif"
            };

            // Act
            var isConsistent = await _variantGenerator.ValidateBrandConsistencyAsync(variant, brandGuidelines);

            // Assert
            Assert.True(isConsistent);
        }

        [Theory]
        [InlineData("<button style='color: red;'>Buy</button>", false)] // Wrong color
        [InlineData("<button style='font-family: Comic Sans;'>Buy</button>", false)] // Wrong font
        public async Task ValidateBrandConsistency_WithInvalidVariant_ShouldReturnFalse(string variant, bool expected)
        {
            // Arrange
            var brandGuidelines = new BrandGuidelines
            {
                PrimaryColor = "#007bff",
                FontFamily = "Arial, sans-serif"
            };

            // Act
            var isConsistent = await _variantGenerator.ValidateBrandConsistencyAsync(variant, brandGuidelines);

            // Assert
            Assert.Equal(expected, isConsistent);
        }
    }
}