using CoreArchitecture.Abstractions.DataAccessInterfaces;
using CoreArchitecture.Data.DBContext;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreArchitecture.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoreArchitectureDBContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        public UnitOfWork(CoreArchitectureDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction = null; // Transaction'ı temizleyin
            }
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction?.Commit();
                _transaction = null; // Transaction'ı temizleyin
            }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction?.Rollback();
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _transaction?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
