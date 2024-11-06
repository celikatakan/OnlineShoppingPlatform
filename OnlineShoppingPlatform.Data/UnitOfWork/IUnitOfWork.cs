using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(); // Saves changes to the database asynchronously, returns the number of affected rows

        Task BeginTransaction();  // Begins a new transaction asynchronously

        Task CommitTransaction();   // Commits the current transaction asynchronously

        Task RollBackTransaction();    // Rolls back the current transaction asynchronously
    }
}
