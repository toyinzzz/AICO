using AICO.Application.Interfaces.Commands;
using AICO.Application.Interfaces.ExternalServices;
using AICO.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Commands
{
    public class GenerateVariantCommandHandler : ICommandHandler<GenerateVariantCommand, List<Variant>>
    {
        private readonly IAIService _aiService;

        public GenerateVariantCommandHandler(IAIService aiService)
        {
            _aiService = aiService;
        }

        public async Task<List<Variant>> Handle(GenerateVariantCommand command)
        {
            var request = new AIVariantRequest(
                command.OriginalPageUrl,
                command.OptimizationGoal,
                command.TargetAudience,
                command.ContentType,
                command.Keywords
            );

            // This is a simplified example. In a real application, you would likely
            // receive more structured data from the AI service and map it to your domain entities.
            var generatedContent = await _aiService.GenerateVariantContentAsync(request);

            // For now, we'll just create a single variant with the generated content.
            // TODO: Get actual abTestId from the command or context
            var abTestId = Guid.NewGuid(); // This should come from the actual AB test
            var variant = new Variant(
                "Variant 1", // name
                abTestId,
                generatedContent,
                50.0m, // 50% traffic allocation
                false // not control variant
            );
            // You would also save this variant to your database here.

            return new List<Variant> { variant };
        }

        public Task<List<Variant>> HandleAsync(GenerateVariantCommand command)
        {
            throw new NotImplementedException();
        }
    }
}