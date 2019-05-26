using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyIMDB.Interfaces;

namespace MyIMDB.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected DbContext DbContext;

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TEntity Add(TEntity entity)
        {
            return DbContext.Add(entity).Entity;
        }

        public IEnumerable<TEntity> Add(IReadOnlyCollection<TEntity> entities)
        {
            return DbContext.Add(entities).Entity;
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

        public IQueryable<TEntity> Get(long id)
        {
            return GetQueryable().Where(x => x.Id == id);
        }
    }
}
