using Microsoft.EntityFrameworkCore;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess
{
    public class MoviePersonRepository : Repository<MoviePerson>, IMoviePersonRepository
    {
        public MoviePersonRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<MoviePerson> GetFull(long id)
        {
            return await DbContext.Set<MoviePerson>().Where(x => x.Id == id)
                .Include(x => x.Country)
                .Include(x => x.Gender)
                .Include(x => x.MoviePersonsMovies)
                    .ThenInclude(x => x.Movie)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<MoviePerson>> GetBySearchQuery(string searchQuery)
        {
            return await DbContext.Set<MoviePerson>().Where(x => x.FullName.Contains(searchQuery))
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}
