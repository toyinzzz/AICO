using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Variant.Create(abTestId, "Control", "Original", 50, ".button {}", true),
                Variant.Create(abTestId, "Variant A", "Modified", 50, ".button { color: red; }", false)
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

        [Fact]
        public async Task GetActiveVariantsAsync_ShouldReturnOnlyActiveVariants()
        {
            // Arrange
            var activeVariants = new List<Variant>
            {
                Variant.Create(Guid.NewGuid(), "Active 1", "Description", 50, ".button {}", true),
                Variant.Create(Guid.NewGuid(), "Active 2", "Description", 50, ".button {}", false)
            };

            _mockRepository.Setup(r => r.GetActiveVariantsAsync())
                .ReturnsAsync(activeVariants);

            // Act
            var result = await _mockRepository.Object.GetActiveVariantsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.All(result, v => Assert.True(v.IsActive));
        }
    }
}