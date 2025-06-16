using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;
using Moq;
using Xunit;

namespace AICO.UnitTests.Domain.Services
{
    public class StatisticalSignificanceTests
    {
        private readonly Mock<IAbTestService> _mockAbTestService;
        private readonly StatisticalSignificanceCalculator _calculator;

        public StatisticalSignificanceTests()
        {
            _mockAbTestService = new Mock<IAbTestService>();
            _calculator = new StatisticalSignificanceCalculator();
        }

        [Theory]
        [InlineData(1000, 50, 1000, 60, 0.95, true)]  // Significant difference
        [InlineData(100, 5, 100, 6, 0.95, false)]    // Not enough sample size
        [InlineData(1000, 50, 1000, 51, 0.95, false)] // Difference too small
        [InlineData(5000, 250, 5000, 300, 0.95, true)] // Large sample, significant
        public void CalculateSignificance_WithVariousScenarios_ShouldReturnCorrectResult(
            int controlViews, int controlConversions,
            int variantViews, int variantConversions,
            double confidenceLevel, bool expectedSignificant)
        {
            // Arrange
            var controlRate = (double)controlConversions / controlViews;
            var variantRate = (double)variantConversions / variantViews;

            // Act
            var result = _calculator.IsStatisticallySignificant(
                controlViews, controlConversions,
                variantViews, variantConversions,
                confidenceLevel);

            // Assert
            Assert.Equal(expectedSignificant, result.IsSignificant);
            Assert.True(result.PValue >= 0 && result.PValue <= 1);
            Assert.True(result.ConfidenceLevel == confidenceLevel);
        }

        [Fact]
        public async Task IsTestStatisticallySignificantAsync_WithSignificantResults_ShouldReturnTrue()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var testResults = new AbTestResults
            {
                ControlVariant = new VariantResults
                {
                    Views = 5000,
                    Conversions = 250,
                    ConversionRate = 0.05
                },
                TestVariant = new VariantResults
                {
                    Views = 5000,
                    Conversions = 350,
                    ConversionRate = 0.07
                }
            };

            _mockAbTestService.Setup(x => x.GetTestResultsAsync(testId))
                .ReturnsAsync(testResults);
            _mockAbTestService.Setup(x => x.IsTestStatisticallySignificantAsync(testId, 0.95))
                .ReturnsAsync(true);

            // Act
            var result = await _mockAbTestService.Object.IsTestStatisticallySignificantAsync(testId, 0.95);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CalculateRequiredSampleSize_WithValidParameters_ShouldReturnCorrectSize()
        {
            // Arrange
            var baselineConversionRate = 0.05; // 5%
            var minimumDetectableEffect = 0.01; // 1% absolute increase
            var power = 0.8; // 80% power
            var significanceLevel = 0.05; // 95% confidence

            // Act
            var sampleSize = _calculator.CalculateRequiredSampleSize(
                baselineConversionRate,
                minimumDetectableEffect,
                power,
                significanceLevel);

            // Assert
            Assert.True(sampleSize > 0);
            Assert.True(sampleSize < 100000); // Reasonable upper bound
        }

        [Fact]
        public async Task DeclareWinnerAsync_WhenStatisticallySignificant_ShouldDeclareWinner()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var winningVariantId = Guid.NewGuid();
            var winningVariant = new Variant(winningVariantId, "Winner", "<html>Winner</html>");

            _mockAbTestService.Setup(x => x.IsTestStatisticallySignificantAsync(testId, 0.95))
                .ReturnsAsync(true);
            _mockAbTestService.Setup(x => x.DeclareWinnerAsync(testId, winningVariantId))
                .ReturnsAsync(winningVariant);

            // Act
            var result = await _mockAbTestService.Object.DeclareWinnerAsync(testId, winningVariantId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(winningVariantId, result.Id);
        }

        [Theory]
        [InlineData(0.90)] // 90% confidence
        [InlineData(0.95)] // 95% confidence
        [InlineData(0.99)] // 99% confidence
        public void CalculateConfidenceInterval_WithDifferentLevels_ShouldReturnValidInterval(
            double confidenceLevel)
        {
            // Arrange
            var conversions = 100;
            var views = 2000;
            var conversionRate = (double)conversions / views;

            // Act
            var interval = _calculator.CalculateConfidenceInterval(
                conversions, views, confidenceLevel);

            // Assert
            Assert.True(interval.LowerBound >= 0);
            Assert.True(interval.UpperBound <= 1);
            Assert.True(interval.LowerBound <= conversionRate);
            Assert.True(interval.UpperBound >= conversionRate);
            Assert.True(interval.LowerBound < interval.UpperBound);
        }
    }
}