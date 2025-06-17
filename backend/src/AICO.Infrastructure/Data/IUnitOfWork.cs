namespace AICO.Infrastructure.Data
{
    /// <summary>
    /// Interface for managing database transactions and coordinating work across multiple repositories
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves all pending changes in the context to the database
        /// </summary>
        /// <returns>The number of objects written to the database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        Task RollbackTransactionAsync();
    }
}