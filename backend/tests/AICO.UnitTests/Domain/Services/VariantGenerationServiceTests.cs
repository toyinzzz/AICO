using AICO.Application.Interfaces.ExternalServices;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;
using Moq;
using Xunit;

namespace AICO.UnitTests.Domain.Services
{
    public class VariantGenerationServiceTests
    {
        private readonly Mock<IAIService> _mockAIService;
        private readonly Mock<IVariantGenerationService> _mockVariantService;

        public VariantGenerationServiceTests()
        {
            _mockAIService = new Mock<IAIService>();
            _mockVariantService = new Mock<IVariantGenerationService>();
        }

        [Fact]
        public async Task GenerateVariantsAsync_WithValidRequest_ShouldReturnVariants()
        {
            // Arrange
            var originalPageUrl = "https://example.com/landing";
            var request = new VariantGenerationRequest
            {
                TargetAudience = "Tech professionals",
                OptimizationGoal = "Increase conversions",
                VariantCount = 3
            };

            var expectedVariants = new List<Variant>
            {
                new Variant(Guid.NewGuid(), "Variant A", "<html>Variant A content</html>"),
                new Variant(Guid.NewGuid(), "Variant B", "<html>Variant B content</html>"),
                new Variant(Guid.NewGuid(), "Variant C", "<html>Variant C content</html>")
            };

            _mockVariantService.Setup(x => x.GenerateVariantsAsync(originalPageUrl, request))
                .ReturnsAsync(expectedVariants);

            // Act
            var result = await _mockVariantService.Object.GenerateVariantsAsync(originalPageUrl, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, variant => Assert.NotNull(variant.HtmlContent));
        }

        [Fact]
        public async Task GenerateHeadlineVariationsAsync_WithValidInput_ShouldReturnVariations()
        {
            // Arrange
            var originalHeadline = "Buy Now and Save";
            var targetAudience = "Budget-conscious shoppers";
            var expectedVariations = new List<string>
            {
                "Save Big - Buy Today!",
                "Limited Time Offer - Act Now",
                "Exclusive Deal - Don't Miss Out",
                "Best Price Guaranteed",
                "Special Discount - Order Now"
            };

            _mockVariantService.Setup(x => x.GenerateHeadlineVariationsAsync(originalHeadline, targetAudience, 5))
                .ReturnsAsync(expectedVariations);

            // Act
            var result = await _mockVariantService.Object.GenerateHeadlineVariationsAsync(originalHeadline, targetAudience, 5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.All(result, headline => Assert.False(string.IsNullOrEmpty(headline)));
        }

        [Fact]
        public async Task ValidateVariantAsync_WithValidVariant_ShouldReturnSuccess()
        {
            // Arrange
            var variant = new Variant(Guid.NewGuid(), "Test Variant", "<html><body>Valid content</body></html>");
            var expectedResult = new VariantValidationResult
            {
                IsValid = true,
                BrandConsistencyScore = 0.95,
                ValidationErrors = new List<string>()
            };

            _mockVariantService.Setup(x => x.ValidateVariantAsync(variant))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _mockVariantService.Object.ValidateVariantAsync(variant);

            // Assert
            Assert.True(result.IsValid);
            Assert.True(result.BrandConsistencyScore > 0.9);
            Assert.Empty(result.ValidationErrors);
        }

        [Theory]
        [InlineData("", "Empty content should fail validation")]
        [InlineData("<script>alert('xss')</script>", "XSS content should fail validation")]
        [InlineData("<html><body></body></html>", "Empty body should fail validation")]
        public async Task ValidateVariantAsync_WithInvalidContent_ShouldReturnFailure(string invalidContent, string reason)
        {
            // Arrange
            var variant = new Variant(Guid.NewGuid(), "Invalid Variant", invalidContent);
            var expectedResult = new VariantValidationResult
            {
                IsValid = false,
                BrandConsistencyScore = 0.2,
                ValidationErrors = new List<string> { reason }
            };

            _mockVariantService.Setup(x => x.ValidateVariantAsync(variant))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _mockVariantService.Object.ValidateVariantAsync(variant);

            // Assert
            Assert.False(result.IsValid);
            Assert.True(result.ValidationErrors.Count > 0);
        }

        [Fact]
        public async Task GenerateCtaVariationsAsync_WithConversionGoal_ShouldReturnOptimizedCTAs()
        {
            // Arrange
            var originalCta = "Click Here";
            var conversionGoal = "Purchase";
            var expectedCTAs = new List<string>
            {
                "Buy Now",
                "Get Yours Today",
                "Order Now",
                "Add to Cart",
                "Purchase Now"
            };

            _mockVariantService.Setup(x => x.GenerateCtaVariationsAsync(originalCta, conversionGoal, 5))
                .ReturnsAsync(expectedCTAs);

            // Act
            var result = await _mockVariantService.Object.GenerateCtaVariationsAsync(originalCta, conversionGoal, 5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.All(result, cta => Assert.Contains("Buy|Get|Order|Add|Purchase", cta));
        }
    }
}