using System;
using System.Linq;
using AICO.Domain.Entities;
using AICO.Domain.Services;
using Xunit;

namespace AICO.Domain.Tests.Services
{
    public class CampaignStateValidationServiceTests
    {
        private readonly CampaignStateValidationService _validationService;

        public CampaignStateValidationServiceTests()
        {
            _validationService = new CampaignStateValidationService();
        }

        [Theory]
        [InlineData(CampaignStatus.Draft, CampaignStatus.Active, true)]
        [InlineData(CampaignStatus.Active, CampaignStatus.Stopped, true)]
        [InlineData(CampaignStatus.Active, CampaignStatus.Completed, true)]
        [InlineData(CampaignStatus.Stopped, CampaignStatus.Active, true)]
        [InlineData(CampaignStatus.Stopped, CampaignStatus.Completed, true)]
        [InlineData(CampaignStatus.Draft, CampaignStatus.Completed, false)]
        [InlineData(CampaignStatus.Completed, CampaignStatus.Active, false)]
        [InlineData(CampaignStatus.Completed, CampaignStatus.Draft, false)]
        public void CanTransitionTo_ShouldReturnExpectedResult(CampaignStatus from, CampaignStatus to, bool expected)
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
                _validationService.ValidateTransition(CampaignStatus.Draft, CampaignStatus.Active));
            
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateTransition_WithInvalidTransition_ShouldThrowInvalidOperationException()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => 
                _validationService.ValidateTransition(CampaignStatus.Draft, CampaignStatus.Completed));
            
            Assert.Contains("Invalid campaign state transition", exception.Message);
            Assert.Contains("Draft", exception.Message);
            Assert.Contains("Completed", exception.Message);
        }

        [Fact]
        public void GetValidTransitions_FromDraft_ShouldReturnActiveOnly()
        {
            // Act
            var transitions = _validationService.GetValidTransitions(CampaignStatus.Draft);

            // Assert
            Assert.Single(transitions);
            Assert.Contains(CampaignStatus.Active, transitions);
        }

        [Fact]
        public void GetValidTransitions_FromCompleted_ShouldReturnEmpty()
        {
            // Act
            var transitions = _validationService.GetValidTransitions(CampaignStatus.Completed);

            // Assert
            Assert.Empty(transitions);
        }
    }
}