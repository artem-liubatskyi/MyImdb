using System;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AutoMapper;
using MyIMDB.DataAccess.Interfaces;

namespace MyIMDB.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork Uow;
        private readonly IMapper mapper;

        public MovieService(IUnitOfWork uow, IMapper mapper)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<MovieViewModel> Get(long id, long? userId)
        {
            var entity = await Uow.Repository<Movie>().GetQueryable().Where(x=>x.Id==id)
                .Include(x => x.MoviesCountries)
                    .ThenInclude(x => x.Country)
                .Include(x=>x.UserMovies)
                .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                .Include(x=>x.MoviePersonsMovies)
                    .ThenInclude(x=>x.Person)
                        .ThenInclude(x=>x.MoviePersonsMovies)
                            .ThenInclude(x=>x.MoviePersonType)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return mapper.Map<Movie, MovieViewModel>(entity, opt => opt.Items.Add("userId", userId));          
        }
        public async Task<IEnumerable<MovieListViewModel>> GetListBySearchQuery(string searchQuerue, long? userId)
        {
            var entity = await Uow.Repository<Movie>().GetQueryable()
                .Where(x => x.Title.Contains(searchQuerue))
                .Include(movie => movie.UserMovies)
                .AsNoTracking()
                .ToArrayAsync();

            return mapper.Map<Movie[], MovieListViewModel[]>(entity, opt => opt.Items.Add("userId", userId));
        }
        public async Task<IEnumerable<MovieListViewModel>> GetTop(long? userId, int topSize = 250)
        {    
            var entity = await Uow.Repository<Movie>().GetQueryable()
                .OrderByDescending(x => x.RatesCount == 0 ? 0 : x.RatesSum / x.RatesCount)
                .Take(topSize)
                .Include(x=>x.UserMovies)
                .AsNoTracking()
                .ToArrayAsync();

            return mapper.Map<Movie[], MovieListViewModel[]>(entity, opt => opt.Items.Add("userId", userId));
        }
        public async Task AddRate(RateViewModel model, long userId)
        {
            var user =  await Uow.Repository<User>().GetQueryable().Where(x=>x.Id==userId)
                .Include(x => x.Movies)
                .FirstAsync();

            var rate = user.Movies.FirstOrDefault(x => x.MovieId == model.MovieId);

            var movie = await Uow.Repository<Movie>().Get(model.MovieId);

            if (movie == null)
                throw new ArgumentException($"Movie with id {model.MovieId} not found");

            if (rate == null)
            {
                await Uow.UserMoviesRepository().Add(new UserMovie { UserId = userId, MovieId = model.MovieId, Rate = model.Value });
                movie.RatesCount++;
                movie.RatesSum += model.Value;
            }
            else
            {
                movie.RatesSum = movie.RatesSum - (int)rate.Rate + model.Value;
                rate.Rate = model.Value;
            }
            await Uow.SaveChangesAsync();
        }
        public async Task<bool> AddToWatchlist(long movieId, long userId)
        {
            var user = await Uow.Repository<User>().GetQueryable().Where(x => x.Id == userId)
                .Include(x => x.Movies)
                .FirstAsync();

            var rate = user.Movies.Where(x => x.MovieId == movieId).FirstOrDefault();
            if (rate == null)
            {
                var movie = await Uow.Repository<Movie>().Get(movieId);
                if (movie == null)
                    return false;
                else
                {
                    await Uow.UserMoviesRepository().Add(new UserMovie { UserId = userId, MovieId = movieId, IsInWatchlist = true});
                    await Uow.SaveChangesAsync();
                    return true;
                }
            }
            else
                rate.IsInWatchlist = true;
            await Uow.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveFromWatchlist(long movieId, long userId)
        {
            var user = await Uow.Repository<User>().GetQueryable().Where(x => x.Id == userId)
                .Include(x => x.Movies)
                .FirstAsync();

            var rate = user.Movies.Where(x => x.MovieId == movieId).FirstOrDefault();
            if (rate == null)
                return false;
            else
                rate.IsInWatchlist = false;
            await Uow.SaveChangesAsync();
            return true;
        }
    }
}
