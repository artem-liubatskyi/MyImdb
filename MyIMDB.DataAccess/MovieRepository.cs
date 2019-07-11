using Microsoft.EntityFrameworkCore;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Movie>> GetBySearchQuery(string searchQuery)
        {
            return await DbContext.Set<Movie>().Where(x => x.Title.Contains(searchQuery))
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Movie>> GetBySearchQueryWithUserMovies(string searchQuery)
        {
            var contains = await DbContext.Set<Movie>().Where(x => x.Title.Contains(searchQuery))
                .Include(movie => movie.UserMovies)
                .AsNoTracking()
                .ToListAsync();

            var equal = contains.FirstOrDefault(x => x.Title == searchQuery);

            contains.Remove(equal);

            contains.Prepend(equal);

            return contains;
        }

        public async Task<Movie> GetFull(long id)
        {
            return await DbContext.Set<Movie>().Where(x => x.Id == id)
                .Include(x => x.MoviesCountries)
                    .ThenInclude(x => x.Country)
                .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.MoviePersonsMovies)
                    .ThenInclude(x => x.Person)
                        .ThenInclude(x => x.MoviePersonsMovies)
                            .ThenInclude(x => x.MoviePersonType)
                .Include(x=>x.Reviews)
                    .ThenInclude(x=>x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        public async Task<Movie> GetFullWithUserMovies(long id)
        {
            return await DbContext.Set<Movie>().Where(x => x.Id == id)
                .Include(x => x.MoviesCountries)
                    .ThenInclude(x => x.Country)
                .Include(x => x.UserMovies)//*
                .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.MoviePersonsMovies)
                    .ThenInclude(x => x.Person)
                        .ThenInclude(x => x.MoviePersonsMovies)
                            .ThenInclude(x => x.MoviePersonType)
                .Include(x => x.Reviews)
                    .ThenInclude(x => x.User)
                        .ThenInclude(x=>x.Likes)//*
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Movie>> GetTopRatedMovies(int topSize = 250)
        {
            return await DbContext.Set<Movie>()
                .OrderByDescending(x => x.RatesCount == 0 ? 0 : x.RatesSum / x.RatesCount)
                .Take(topSize)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Movie>> GetTopRatedMoviesWithUserMovies(int topSize = 250)
        {
            return await DbContext.Set<Movie>()
                .OrderByDescending(x => x.RatesCount == 0 ? 0 : x.RatesSum / x.RatesCount)
                .Take(topSize)
                .Include(x => x.UserMovies)
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}
