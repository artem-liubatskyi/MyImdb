using System.Threading.Tasks;
using MyIMDB.Data.Abstraction;

namespace MyIMDB.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
        IUserMovieRepository UserMoviesRepository();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task<int> SaveChangesAsync();
    }
}
