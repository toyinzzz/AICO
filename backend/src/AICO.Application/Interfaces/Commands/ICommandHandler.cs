using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Commands
{
    /// <summary>
    /// Defines a handler for a command that does not return a result.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to be handled.</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }

    /// <summary>
    /// Defines a handler for a command that returns a result.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to be handled.</typeparam>
    /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}