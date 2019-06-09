using System.Collections.Generic;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;

namespace MyIMDB.Services
{
    public interface IMovieService
    {
        Task<MovieViewModel> Get(long id, long? userId);
        Task<IEnumerable<MovieListViewModel>> GetListBySearchQuery(string searchQuerue, long? userId);
        Task AddRate(RateViewModel model, long userId);
        Task<bool> AddToWatchlist(long movieId, long userId);
        Task<bool> RemoveFromWatchlist(long movieId, long userId);
        Task<IEnumerable<MovieListViewModel>> GetTop(long? userId, int topSize=250);
    }
}
