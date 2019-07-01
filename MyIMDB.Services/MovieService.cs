using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIMDB.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork Uow;

        public MovieService(IUnitOfWork uow, IMapper mapper)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<Movie> Get(long id, long? userId)
        {
            Movie entity;

            if (userId == null)
                entity = await Uow.MovieRepository.GetFull(id);
            else
                entity = await Uow.MovieRepository.GetFullWithUserMovies(id);

            return entity;
        }
        public async Task<IEnumerable<Movie>> GetListBySearchQuery(string searchQuery, long? userId)
        {
            IEnumerable<Movie> entity;

            if (userId == null)
                entity = await Uow.MovieRepository.GetBySearchQuery(searchQuery);
            else
                entity = await Uow.MovieRepository.GetBySearchQueryWithUserMovies(searchQuery);

            return entity;
        }
        public async Task<IEnumerable<Movie>> GetTop(long? userId, int topSize = 250)
        {
            IEnumerable<Movie> entity;

            if (userId == null)
                entity = await Uow.MovieRepository.GetTopRatedMovies(topSize);
            else
                entity = await Uow.MovieRepository.GetTopRatedMoviesWithUserMovies(topSize);

            return entity;
        }
        public async Task AddRate(RateViewModel model, long userId)
        {
            var user = await Uow.UserRepository.GetWithMovies(userId);

            var rate = user.Movies.FirstOrDefault(x => x.MovieId == model.MovieId);

            var movie = await Uow.MovieRepository.Get(model.MovieId);

            if (movie == null)
                throw new ArgumentException($"Movie with id {model.MovieId} not found");

            if (rate == null)
            {
                await Uow.UserMoviesRepository.Add(new UserMovie { UserId = userId, MovieId = model.MovieId, Rate = model.Value });
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
            var user = await Uow.UserRepository.GetWithMovies(userId);

            var rate = user.Movies.Where(x => x.MovieId == movieId).FirstOrDefault();

            if (rate == null)
            {
                var movie = await Uow.MovieRepository.Get(movieId);
                if (movie == null)
                    return false;
                else
                {
                    await Uow.UserMoviesRepository.Add(new UserMovie { UserId = userId, MovieId = movieId, IsInWatchlist = true });
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
            var user = await Uow.UserRepository.GetWithMovies(userId);

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
