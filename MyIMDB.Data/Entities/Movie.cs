using MyIMDB.Data.Abstraction;
using System.Collections.Generic;

namespace MyIMDB.Data.Entities
{
    public class Movie : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public virtual IEnumerable<MoviePersonsMovies> MoviePersonsMovies { get; set; }

        public virtual IEnumerable<MoviesCountries> MoviesCountries { get; set; }

        public virtual IEnumerable<MoviesGenres> Genres { get; set; }

        public virtual IEnumerable<UserMovie> UserMovies { get; set; }

        public long RatesSum { get; set; }

        public long RatesCount { get; set; }
    }
}
