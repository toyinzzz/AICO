using System;
using AICO.Domain.Entities;
using AICO.Domain.Services;
using Xunit;

namespace AICO.Domain.Tests.Services
{
    public class AbTestStateValidationServiceTests
    {
        private readonly AbTestStateValidationService _validationService;

        public AbTestStateValidationServiceTests()
        {
            _validationService = new AbTestStateValidationService();
        }

        [Theory]
        [InlineData(AbTestStatus.Draft, AbTestStatus.Running, true)]
        [InlineData(AbTestStatus.Running, AbTestStatus.Stopped, true)]
        [InlineData(AbTestStatus.Running, AbTestStatus.Completed, true)]
        [InlineData(AbTestStatus.Stopped, AbTestStatus.Running, true)]
        [InlineData(AbTestStatus.Stopped, AbTestStatus.Completed, true)]
        [InlineData(AbTestStatus.Draft, AbTestStatus.Completed, false)]
        [InlineData(AbTestStatus.Completed, AbTestStatus.Running, false)]
        [InlineData(AbTestStatus.Completed, AbTestStatus.Draft, false)]
        public void CanTransitionTo_ShouldReturnExpectedResult(AbTestStatus from, AbTestStatus to, bool expected)
        {
            // Act
            var result = _validationService.CanTransitionTo(from, to);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValidateTransition_WithValidTransition_ShouldNotThrow()
        {
            // Act & Assert
            var exception = Record.Exception(() => 
                _validationService.ValidateTransition(AbTestStatus.Draft, AbTestStatus.Running));
            
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateTransition_WithInvalidTransition_ShouldThrowInvalidOperationException()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => 
                _validationService.ValidateTransition(AbTestStatus.Draft, AbTestStatus.Completed));
            
            Assert.Contains("Invalid state transition", exception.Message);
        }

        [Fact]
        public void GetValidTransitions_ForDraftStatus_ShouldReturnCorrectTransitions()
        {
            // Act
            var validTransitions = _validationService.GetValidTransitions(AbTestStatus.Draft);

            // Assert
            Assert.Contains(AbTestStatus.Running, validTransitions);
            Assert.DoesNotContain(AbTestStatus.Completed, validTransitions);
        }
    }
}