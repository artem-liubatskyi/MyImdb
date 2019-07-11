using System.Collections.Generic;
using MyIMDB.Data.Abstraction;

namespace MyIMDB.Data.Entities
{
    public class MoviePersonType : IEntity
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public virtual IEnumerable<MoviePersonsMovies> MoviePersonsMovies { get; set; }
    }
}
