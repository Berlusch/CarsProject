using CarsProject.DAL;
using CarsProject.Repository.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarsProject.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarsDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(CarsDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
