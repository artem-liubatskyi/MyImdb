using System.Collections.Generic;

namespace TmdbClient.ApiModels
{
    public class FindMovieResult
    {
        public int Page { get; set; }
        public List<MovieSearch> results { get; set; }
        public int Total_pages { get; set; }
        public int Total_results { get; set; }
    }
}
