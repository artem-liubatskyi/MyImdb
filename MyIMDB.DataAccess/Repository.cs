using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyIMDB.Data.Abstraction;
using MyIMDB.DataAccess.Interfaces;

namespace MyIMDB.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected DbContext DbContext;

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            await DbContext.AddAsync(entity);
            return entity;
        }
        public async Task<IEnumerable<TEntity>> Add(IReadOnlyCollection<TEntity> entities)
        {
            await DbContext.AddAsync(entities);
            return entities;
        }
        public TEntity Update(TEntity entity)
        {
            return DbContext.Update(entity).Entity;
        }
        public IQueryable<TEntity> GetQueryable()
        {
            return DbContext.Set<TEntity>();
        }

        public void Delete(TEntity entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Attach(entity);
            }

            DbContext.Remove(entity);
        }

        public void Delete(IReadOnlyCollection<TEntity> entities)
        {
            if (DbContext.Entry(entities).State == EntityState.Detached)
            {
                DbContext.Attach(entities);
            }

            DbContext.Remove(entities);
        }

        public async Task<TEntity> Get(long id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }
    }
}
