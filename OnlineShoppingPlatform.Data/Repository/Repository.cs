using Microsoft.EntityFrameworkCore;
using OnlineShoppingPlatform.Data.Context;
using OnlineShoppingPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Data.Repository
{
    // Generic repository class implementing basic CRUD operations for entities
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ShoppingPlatformDbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        // Constructor initializes database context and DbSet
        public Repository(ShoppingPlatformDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }
        // Adds a new entity to the database, setting the CreatedDate to the current time
        public void Add(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            _dbSet.Add(entity);

        }
        // Deletes an entity from the database, with optional soft delete
        public void Delete(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.ModifiedDate = DateTime.Now;
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
                _dbSet.Remove(entity);
        }
        // Deletes an entity by ID, defaulting to soft delete
        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }
        // Retrieves a single entity matching the specified condition
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }
        // Retrieves all entities, with an optional condition to filter results
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is null ? _dbSet : _dbSet.Where(predicate);
        }
        // Retrieves a single entity by its ID
        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }
        // Updates an existing entity, setting the ModifiedDate to the current time
        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);

        }

        public async Task<bool> UserExistAsync(int customerId)
        {
           return await _db.Users.AnyAsync(x=> x.Id == customerId);
        }
    }
}
