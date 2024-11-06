using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class  // Interface defining CRUD operations for a repository
    {
        Task<bool> UserExistAsync(int customerId);
        void Add(TEntity entity);   // Adds a new entity to the repository
        void Delete(TEntity entity, bool softDelete = true);      // Deletes an entity, with an optional parameter for soft delete
        void Delete(int id);      // Deletes an entity by ID, typically performing a soft delete
        void Update(TEntity entity); // Updates an existing entity in the repository
        TEntity GetById(int id); // Retrieves a single entity by its ID
        TEntity Get(Expression<Func<TEntity, bool>> predicate);  // Retrieves a single entity based on a specified condition

        // Retrieves all entities, optionally filtered by a specified condition
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);



    }
}
