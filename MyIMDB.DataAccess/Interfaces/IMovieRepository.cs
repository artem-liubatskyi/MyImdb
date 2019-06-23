using MyIMDB.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<Movie> GetFull(long id);
        Task<Movie> GetFullWithUserMovies(long id);
        Task<IEnumerable<Movie>> GetTopRatedMovies(int topSize = 250);
        Task<IEnumerable<Movie>> GetTopRatedMoviesWithUserMovies(int topSize = 250);
        Task<IEnumerable<Movie>> GetBySearchQuery(string searchQuery);
        Task<IEnumerable<Movie>> GetBySearchQueryWithUserMovies(string searchQuery);
    }
}
