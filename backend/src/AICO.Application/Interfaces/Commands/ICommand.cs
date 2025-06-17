namespace AICO.Application.Interfaces.Commands
{
    /// <summary>
    /// Marker interface for all commands.
    /// </summary>
    public interface ICommand
    {
    }

    /// <summary>
    /// Marker interface for commands that return a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
    public interface ICommand<out TResult> // 'out' for covariance, as TResult is only used as a return type indication
    {
    }
}