namespace MyIMDB.Data.Entities
{
    public class MoviesCountries
    {
        public long MovieId { get; set; }
        public Movie Movie { get; set; }

        public long CountryId { get; set; }
        public Country Country { get; set; }
    }
}
