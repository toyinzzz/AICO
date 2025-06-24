using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace AICO.Domain.Tests.Repositories
{
    public class VariantRepositoryTests
    {
        private readonly Mock<IVariantRepository> _mockRepository;

        public VariantRepositoryTests()
        {
            _mockRepository = new Mock<IVariantRepository>();
        }

        [Fact]
        public async Task GetByAbTestIdAsync_WithValidId_ShouldReturnVariants()
        {
            // Arrange
            var abTestId = Guid.NewGuid();
            var variants = new List<Variant>
            {
                // Constructor: string name, Guid abTestId, string content, int trafficAllocation, bool isControl = false, string aiPrompt = null, int? aiConfidenceScore = null
                new Variant("Control", abTestId, ".button {}", 50, true, "Original"),
                new Variant("Variant A", abTestId, ".button { color: red; }", 50, false, "Modified")
            };

            _mockRepository.Setup(r => r.GetByAbTestIdAsync(abTestId))
                .ReturnsAsync(variants);

            // Act
            var result = await _mockRepository.Object.GetByAbTestIdAsync(abTestId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, v => Assert.Equal(abTestId, v.AbTestId));
        }

        // [Fact] // Commenting out due to non-existent GetActiveVariantsAsync in IVariantRepository and IsActive property in Variant
        // public async Task GetActiveVariantsAsync_ShouldReturnOnlyActiveVariants()
        // {
        //     // Arrange
        //     var activeVariants = new List<Variant>
        //     {
        //         // Constructor: string name, Guid abTestId, string content, int trafficAllocation, bool isControl = false, string aiPrompt = null, int? aiConfidenceScore = null
        //         new Variant("Active 1", Guid.NewGuid(), ".button {}", 50, true, "Description"),
        //         new Variant("Active 2", Guid.NewGuid(), ".button {}", 50, false, "Description")
        //     };

        //     _mockRepository.Setup(r => r.GetActiveVariantsAsync())
        //         .ReturnsAsync(activeVariants);

        //     // Act
        //     var result = await _mockRepository.Object.GetActiveVariantsAsync();

        //     // Assert
        //     Assert.NotNull(result);
        //     // Assert.All(result, v => Assert.True(v.IsActive)); // IsActive property does not exist, IsControl is available.
        // }
    }
}