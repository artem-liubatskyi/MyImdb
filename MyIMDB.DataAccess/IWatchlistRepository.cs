using System.Linq;
using System.Threading.Tasks;
using MyIMDB.Data.Entities;

namespace MyIMDB.DataAccess
{
    public interface IWatchlistRepository
    {
        Task<WatchLaterMovies> AddAsync(WatchLaterMovies entity);
        void Remove(WatchLaterMovies entity);
        IQueryable<WatchLaterMovies> GetQueryable();
    }
}
