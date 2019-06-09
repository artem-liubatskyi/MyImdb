using System;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MyIMDB.DataAccess;
using AutoMapper;

namespace MyIMDB.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork Uow;
        private readonly IRateRepository rateRepository;
        private readonly IMapper mapper;
        private readonly IWatchlistRepository watchlistRepository;

        public MovieService(IUnitOfWork uow, IRateRepository rateRepository, IMapper mapper, IWatchlistRepository watchlistRepository)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
            this.rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.watchlistRepository = watchlistRepository ?? throw new ArgumentNullException(nameof(watchlistRepository));
        }

        private int getUserRate(Movie movie, long? userId)
        {
            Rate rate = null;
            
            rate = movie.Rates.FirstOrDefault(r => r.ProfileId == userId);
            if (rate != null)
               return rate.Value;
            
            return 0;
        }
        private double getAverageRate(Movie movie)
        {
            return movie.Rates.Any() ? movie.Rates.Sum(rate => rate.Value) / movie.Rates.Count() : 0;
        }

        public async Task<MovieViewModel> Get(long id, long? userId)
        {
            var entity = await Uow.Repository<Movie>().GetQueryable().Where(x=>x.Id==id)
                .Include(x => x.MoviesCountries)
                    .ThenInclude(x => x.Country)
                .Include(x=>x.Rates)
                .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                .Include(x=>x.MoviePersonsMovies)
                    .ThenInclude(x=>x.Person)
                        .ThenInclude(x=>x.MoviePersonsMovies)
                            .ThenInclude(x=>x.MoviePersonType)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var model = mapper.Map<MovieViewModel>(entity);

            await Task.Run(()=>
            {
                model.Genres = entity.Genres.Select(x => x.Genre).Select(x => x.Title).ToArray();

                model.Directers = mapper.ProjectTo<MoviePersonListViewModel>
                    (entity.MoviePersonsMovies.Where(x => x.MoviePersonType.Type == Constants.DirecterType).AsQueryable());

                model.Stars = mapper.ProjectTo<MoviePersonListViewModel>
                    (entity.MoviePersonsMovies.Where(x => x.MoviePersonType.Type == Constants.StarType).AsQueryable());

                model.AverageRate = entity.Rates.Any() ? entity.Rates.Sum(x => x.Value) / entity.Rates.Count() : 0;

                model.UsersRate = getUserRate(entity, userId);

                model.Countries = entity.MoviesCountries.Select(x => x.Country.Name);
            });

            return model;
        }
        public async Task<IEnumerable<MovieListViewModel>> GetListBySearchQuery(string searchQuerue, long? userId)
        {
            return await Uow.Repository<Movie>().GetQueryable()
                .Where(x => x.Title.Contains(searchQuerue))
                .Include(movie => movie.Rates)   
                .Select(x => new MovieListViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Year = x.Year,
                    AverageRate = (x.Rates.Any() ? x.Rates.Sum(rate => rate.Value) / x.Rates.Count() : 0),
                    ImageUrl = x.ImageUrl,
                    UsersRate = (x.Rates.Any() & userId != null) ? (x.Rates.FirstOrDefault(rate => rate.ProfileId == userId).Value) : 0
                })
                .ToArrayAsync();
        }
        public async Task<IEnumerable<MovieListViewModel>> GetTop(long? userId, int topSize = 250)
        {
            return await Uow.Repository<Movie>().GetQueryable()
                .Include(movie => movie.Rates)
                .Select(x => new MovieListViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Year = x.Year,
                    AverageRate = (x.Rates.Any() ? x.Rates.Sum(rate => rate.Value) / x.Rates.Count() : 0),
                    ImageUrl = x.ImageUrl,
                    UsersRate = (x.Rates.Any() & userId != null) ? (x.Rates.FirstOrDefault(rate => rate.ProfileId == userId).Value) : 0
                })
                .OrderByDescending(x=>x.AverageRate)
                .Take(topSize)
                .ToArrayAsync();
        }
        public async Task AddRate(RateViewModel model, long userId)
        {
            var movie =  await Uow.Repository<Movie>().GetQueryable().Where(x=>x.Id==model.MovieId)
                .Include(x => x.Rates)
                .FirstOrDefaultAsync();

            if (movie != null)
            {
                var rate = movie.Rates.FirstOrDefault(x => x.ProfileId == userId);
                var newRate = new Rate() { MovieId = model.MovieId, ProfileId = userId, Value = model.Value };
                if (rate == null)
                {
                    rateRepository.Add(newRate);
                    movie.Rates.ToList().Add(newRate);

                }
                else
                   rate.Value = newRate.Value;
                await Uow.SaveChangesAsync();
            }
            else
                throw new Exception("No movie with id: " + model.MovieId);
        }

        public async Task<bool> AddToWatchlist(long movieId, long userId)
        {
            var entity = await GetWatchLaterMoviesAsync(movieId, userId);

            if (entity == null)
                return false;

            await watchlistRepository.AddAsync(entity);

            await Uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromWatchlist(long movieId, long userId)
        {
            var entity = await GetWatchLaterMoviesAsync(movieId, userId);

            if (entity == null)
                return false;

            
            watchlistRepository.Remove(entity);
            await Uow.SaveChangesAsync();
            return true;
        }

        private async Task<WatchLaterMovies> GetWatchLaterMoviesAsync(long movieId, long userId)
        {
            var movie = await Uow.Repository<Movie>().Get(movieId);

            if (movie == null)
                return null;

            var user = await Uow.Repository<User>().Get(userId);

            if (user == null)
                return null;

            var entity = new WatchLaterMovies
            {
                Movie = movie,
                MovieId = movieId,
                User = user,
                UsereId = userId
            };
            //user.WatchLaterList = new List<WatchLaterMovies>();
            //user.WatchLaterList.ToList().Add(entity);

            return entity;
        }
    }
}
