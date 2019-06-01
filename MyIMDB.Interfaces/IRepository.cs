using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIMDB.Interfaces
{
    public interface IRepository<TEntity> where TEntity :  IEntity
    {
        Task<TEntity> Add(TEntity entity);
        Task<IEnumerable<TEntity>> Add(IReadOnlyCollection<TEntity> entities);

        Task<TEntity> Get(long id);
        IQueryable<TEntity> GetQueryable();

        TEntity Update(TEntity entity);
        
        void Delete(TEntity entity);
        void Delete(IReadOnlyCollection<TEntity> entities);
    }
}
