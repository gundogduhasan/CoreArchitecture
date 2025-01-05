namespace CoreArchitecture.Abstractions.DataAccessInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        int SaveChanges();

        Task CommitAsync();
        void Commit();

        Task BeginTransactionAsync();
        void BeginTransaction();

        Task RollbackAsync();
        void Rollback();
    }
}
