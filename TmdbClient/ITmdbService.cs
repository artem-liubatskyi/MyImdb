using System.Collections.Generic;
using System.Threading.Tasks;
using TmdbClient.ApiModels;

namespace TmdbClient
{
    public interface ITmdbService
    {
        Task AddMovie(string title);
        Task<List<Person>> GetDirecters(Credits credits);
        Task<TmdbMovie> GetMovieAsync(string title);
        Task<Credits> GetMovieCredits(long movieId);
        Task<Person> GetPersonAsync(long personId);
        Task<List<Person>> GetStars(Credits credits);
    }
}