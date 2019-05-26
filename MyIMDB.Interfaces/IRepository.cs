using System.Collections.Generic;
using System.Linq;

namespace MyIMDB.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        IQueryable<TEntity> Get(long id);
        IEnumerable<TEntity> Add(IReadOnlyCollection<TEntity> entities);
        IQueryable<TEntity> GetQueryable();
        void Delete(TEntity entity);
        void Delete(IReadOnlyCollection<TEntity> entities);
    }
}
