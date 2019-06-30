using System.Threading.Tasks;
using TmdbClient.ApiModels;

namespace TmdbClient
{
    public interface ITmdbClient
    {
        Task<FindMovieResult> FindMovieAsync(string query);
        Task<Credits> GetCreditsAsync(long movieId);
        Task<TmdbMovie> GetMovieByIdAsync(long movieId);
        Task<Person> GetPersonByIdAsync(long personId);
    }
}