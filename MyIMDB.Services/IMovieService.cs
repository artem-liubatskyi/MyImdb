using System.Collections.Generic;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;

namespace MyIMDB.Services
{
    public interface IMovieService
    {
        Task<Movie> Get(long id, long? userId);
        Task<IEnumerable<Movie>> GetListBySearchQuery(string searchQuery, long? userId);
        Task AddRate(RateViewModel model, long userId);
        Task<bool> AddToWatchlist(long movieId, long userId);
        Task<bool> RemoveFromWatchlist(long movieId, long userId);
        Task<IEnumerable<Movie>> GetTop(long? userId, int topSize=250);
        Task AddReview(Review review);
    }
}
