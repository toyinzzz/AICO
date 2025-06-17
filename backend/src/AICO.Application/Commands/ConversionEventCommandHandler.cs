using AICO.Application.Interfaces.Commands;
using AICO.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace AICO.Application.Commands
{
    public class ConversionEventCommandHandler : IConversionEventCommandHandler
    {
        private readonly IConversionEventRepository _conversionEventRepository;

        public ConversionEventCommandHandler(IConversionEventRepository conversionEventRepository)
        {
            _conversionEventRepository = conversionEventRepository ?? throw new ArgumentNullException(nameof(conversionEventRepository));
        }

        public Task HandleAsync(TrackConversionCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            // Add validation as needed
            // Map command to domain entity and save
            throw new NotImplementedException();
        }
    }
}