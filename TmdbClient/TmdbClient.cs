using System.Net.Http;
using System.Threading.Tasks;
using TmdbClient.ApiModels;

namespace TmdbClient
{
    public class TmdbClient : ITmdbClient
    {
        private readonly HttpClient client;

        public TmdbClient()
        {
            this.client = new HttpClient();
        }
        public async Task<TmdbMovie> GetMovieByIdAsync(long id)
        {
            TmdbMovie movie = null;
            var response = await client.GetAsync(Settings.GetMovieByIdUrl(id));
            if (response.IsSuccessStatusCode)
            {
                movie = await response.Content.ReadAsAsync<TmdbMovie>();
            }
            return movie;
        }
        public async Task<FindMovieResult> FindMovieAsync(string query)
        {
            FindMovieResult movies = null;
            var response = await client.GetAsync(Settings.GetMovieSearchUrl(query));
            if (response.IsSuccessStatusCode)
            {
                movies = await response.Content.ReadAsAsync<FindMovieResult>();
            }
            return movies;
        }
        public async Task<Person> GetPersonByIdAsync(long id)
        {
            Person person = null;
            var response = await client.GetAsync(Settings.GetPersonByIdUrl(id));
            if (response.IsSuccessStatusCode)
            {
                person = await response.Content.ReadAsAsync<Person>();
            }
            return person;
        }
        public async Task<Credits> GetCreditsAsync(long id)
        {
            Credits credits = null;
            var response = await client.GetAsync(Settings.GetMovieCastUrl(id));
            if (response.IsSuccessStatusCode)
            {
                credits = await response.Content.ReadAsAsync<Credits>();
            }
            return credits;
        }
    }
}
