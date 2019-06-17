using System.Collections.Generic;
using MyIMDB.Data.Abstraction;

namespace MyIMDB.Data.Entities
{
    public class Genre : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public virtual IEnumerable<MoviesGenres> MoviesGenres { get; set; }
    }
}
