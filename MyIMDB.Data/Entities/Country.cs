using System.Collections.Generic;
using MyIMDB.Interfaces;

namespace MyIMDB.Data.Entities
{
    public class Country : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<MoviesCountries> MoviesCountries { get; set; }
    }
}
