using AICO.Domain.Services;
using Xunit;

namespace AICO.UnitTests.Services
{
    public class StatisticalAnalysisTests
    {
        private readonly StatisticalAnalysisService _analysisService;

        public StatisticalAnalysisTests()
        {
            _analysisService = new StatisticalAnalysisService();
        }

        [Theory]
        [InlineData(1000, 50, 1000, 75, true)] // Clear winner
        [InlineData(100, 5, 100, 7, false)] // Not enough data
        [InlineData(1000, 50, 1000, 52, false)] // Too close
        public void CalculateSignificance_WithVariousScenarios_ShouldReturnCorrectResult(int controlVisitors, int controlConversions, int variantVisitors, int variantConversions, bool expectedSignificant)
        {
            // Act
            var result = _analysisService.CalculateSignificance(controlVisitors, controlConversions, variantVisitors, variantConversions);

            // Assert
            Assert.Equal(expectedSignificant, result.IsSignificant);
            Assert.True(result.PValue >= 0 && result.PValue <= 1);
            Assert.True(result.ConfidenceLevel >= 0 && result.ConfidenceLevel <= 100);
        }

        /*
        [Fact]
        public void DetermineWinner_WithSignificantDifference_ShouldReturnCorrectWinner()
        {
            // Arrange
            var controlStats = new VariantStats
            {
                Visitors = 1000,
                Conversions = 50,
                Revenue = 5000m
            };

            var variantStats = new VariantStats
            {
                Visitors = 1000,
                Conversions = 75,
                Revenue = 7500m
            };

            // Act
            var winner = _analysisService.DetermineWinner(controlStats, variantStats);

            // Assert
            Assert.Equal(WinnerType.Variant, winner.WinnerType);
            Assert.True(winner.ConfidenceLevel > 95);
            Assert.True(winner.ImprovementPercentage > 0);
        }
        */

        // [Theory]
        // [InlineData(100, 5, false)] // Too few conversions
        // [InlineData(500, 25, false)] // Borderline
        // [InlineData(1000, 50, true)] // Sufficient
        // [InlineData(2000, 100, true)] // More than sufficient
        // public void CheckTestCompletionCriteria_WithVariousData_ShouldReturnCorrectResult(int visitors, int conversions, bool expectedComplete)
        // {
        //     // Arrange
        //     var testStats = new AbTestStats
        //     {
        //         TotalVisitors = visitors,
        //         TotalConversions = conversions,
        //         StartDate = DateTime.UtcNow.AddDays(-14),
        //         MinimumDetectableEffect = 0.1m, // 10%
        //         StatisticalPower = 0.8m // 80%
        //     };

        //     // Act
        //     var isComplete = _analysisService.CheckTestCompletionCriteria(testStats);

        //     // Assert
        //     Assert.Equal(expectedComplete, isComplete.IsComplete);
        //     if (isComplete.IsComplete)
        //     {
        //         Assert.NotNull(isComplete.Reason);
        //     }
        // }

        [Fact]
        public void CalculateRequiredSampleSize_WithStandardParameters_ShouldReturnReasonableSize()
        {
            // Arrange
            var baselineConversionRate = 0.05; // 5%
            var minimumDetectableEffect = 0.2; // 20% relative improvement
            var statisticalPower = 0.8; // 80%
            var significanceLevel = 0.05; // 95% confidence

            // Act
            var sampleSize = _analysisService.CalculateMinimumSampleSize(
                baselineConversionRate,
                minimumDetectableEffect,
                statisticalPower,
                significanceLevel);

            // Assert
            Assert.True(sampleSize > 0);
            Assert.True(sampleSize < 100000); // Reasonable upper bound
        }

        // [Fact]
        // public void CalculateBayesianProbability_WithClearWinner_ShouldReturnHighProbability()
        // {
        //     // Arrange
        //     var controlConversions = 50;
        //     var controlVisitors = 1000;
        //     var variantConversions = 75;
        //     var variantVisitors = 1000;

        //     // Act
        //     var probability = _analysisService.CalculateBayesianProbability(
        //         controlConversions, controlVisitors,
        //         variantConversions, variantVisitors);

        //     // Assert
        //     Assert.True(probability > 0.95m); // High probability variant is better
        // }
    }
}