using System.Collections.Generic;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Abstraction
{
    public interface IMoviePerson : IEntity, IPerson
    {
        IEnumerable<MoviePersonsMovies> MoviePersonsMovies { get; set; }
    }
}
