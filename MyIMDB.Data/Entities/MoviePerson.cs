using System;
using System.Collections.Generic;
using MyIMDB.Data.Abstraction;

namespace MyIMDB.Data.Entities
{
    public class MoviePerson : IMoviePerson
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public virtual IEnumerable<MoviePersonsMovies> MoviePersonsMovies { get; set; }

        public string Biography { get; set; }

        public long? GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        public long? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public string ImageUrl { get; set; }
    }
}
