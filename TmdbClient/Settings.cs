namespace TmdbClient
{
    public class Settings
    {
        private const string HostUrl = "https://api.themoviedb.org/3/";
        private const string ImagesHostUrl = "https://image.tmdb.org/t/p/original/";
        private const string MovieSearchPath = "search/movie?";
        private const string MovieByIdPath = "movie/";
        private const string PersonSearchPath = "search/person?";
        private const string PersonByIdPath = "person/";
        private const string ApiKey = "a1ac630338945ad1774e461d396c406b";

        public static string GetMovieSearchUrl(string query, string language = "en-US", int page = 1, bool includeAdult = true)
        {
            return
                $"{HostUrl}{MovieSearchPath}api_key={ApiKey}&language={language}&query={query}&page={page}&include_adult={includeAdult}";
            //https://api.themoviedb.org/3/search/movie
            //    ?api_key=a1ac630338945ad1774e461d396c406b
            //        &language=en-US
            //        &query=shawshank
            //        &page=1
            //        &include_adult=false
        }
        public static string GetMovieByIdUrl(long id, string language = "en-US")
        {
            return
                $"{HostUrl}{MovieByIdPath}{id}?api_key={ApiKey}&language={language}";
            //https://api.themoviedb.org/3/movie/5512?api_key=a1ac630338945ad1774e461d396c406b&language=en-US
        }
        public static string GetPersonSearchUrl(string query, string language = "en-US", int page = 1, bool includeAdult = true)
        {
            return
                $"{HostUrl}{PersonSearchPath}api_key={ApiKey}&language={language}&query={query}&page={page}&include_adult={includeAdult}";

        }
        public static string GetPersonByIdUrl(long id, string language = "en-US")
        {
            return
                $"{HostUrl}{PersonByIdPath}{id}?api_key={ApiKey}&language={language}";
        }
        public static string GetImageUrl(string imagePath)
        {
            return $"{ImagesHostUrl}{imagePath}";
            //https://image.tmdb.org/t/p/original/vfbfcqINRHzXNTenycHIjNO6Va7.jpg
        }
        public static string GetMovieCastUrl(long movieId)
        {
            return $"{HostUrl}movie/{movieId}/credits?api_key={ApiKey}";
        }

    }
}
