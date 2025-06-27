using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Queries
{
    /// <summary>
    /// Defines a handler for a query.
    /// </summary>
    /// <typeparam name="TQuery">The type of query to be handled.</typeparam>
    /// <typeparam name="TResult">The type of result returned by the query.</typeparam>
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}