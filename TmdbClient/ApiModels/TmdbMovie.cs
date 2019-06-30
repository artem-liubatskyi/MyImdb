using System.Collections.Generic;

namespace TmdbClient.ApiModels
{
    public class TmdbMovie
    {
        public bool Adult { get; set; }
        public string Backdrop_path { get; set; }
        public object Belongs_to_collection { get; set; }
        public long Budget { get; set; }
        public List<Genre> Genres { get; set; }
        public object Homepage { get; set; }
        public long Id { get; set; }
        public string Imdb_id { get; set; }
        public string Original_language { get; set; }
        public string Original_title { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public string Poster_path { get; set; }
        public List<ProductionCompany> Production_companies { get; set; }
        public List<ProductionCountry> Production_countries { get; set; }
        public string Release_date { get; set; }
        public long Revenue { get; set; }
        public long Runtime { get; set; }
        public List<SpokenLanguage> Spoken_languages { get; set; }
        public string Status { get; set; }
        public string Tagline { get; set; }
        public string Title { get; set; }
        public bool Video { get; set; }
        public double Vote_average { get; set; }
        public long Vote_count { get; set; }
    }
}
