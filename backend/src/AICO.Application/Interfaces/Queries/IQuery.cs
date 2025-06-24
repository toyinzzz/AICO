namespace AICO.Application.Interfaces.Queries
{
    /// <summary>
    /// Marker interface for all queries that return a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the query.</typeparam>
    public interface IQuery<out TResult> // 'out' for covariance
    {
    }
}