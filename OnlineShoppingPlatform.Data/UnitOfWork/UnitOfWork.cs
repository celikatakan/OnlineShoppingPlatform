using Microsoft.EntityFrameworkCore.Storage;
using OnlineShoppingPlatform.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.UnitOfWork
{
    // Unit of Work class for handling database transactions
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShoppingPlatformDbContext _db;

        private IDbContextTransaction _transaction;

        // Constructor injecting the DbContext dependency
        public UnitOfWork(ShoppingPlatformDbContext db)
        {
            _db = db;
        }
        // Begins a new transaction asynchronously
        public async Task BeginTransaction()
        {
           _transaction = await _db.Database.BeginTransactionAsync();
        }
        // Commits the current transaction asynchronously
        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
        }
        // Disposes the DbContext resources
        public void Dispose()
        {
            _db.Dispose();
        }
        // Rolls back the current transaction asynchronously
        public async Task RollBackTransaction()
        {
           await _transaction.RollbackAsync();
        }
        // Saves changes to the database asynchronously
        public async Task<int> SaveChangesAsync()
        {
           return await _db.SaveChangesAsync();
        }
    }
}
