using System;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MyIMDB.DataAccess;

namespace MyIMDB.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork Uow;
        private readonly IRateRepository rateRepository;

        public MovieService(IUnitOfWork uow, IRateRepository repositiry)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
            rateRepository = repositiry ?? throw new ArgumentNullException(nameof(repositiry));
        }
        public async Task<MovieViewModel> Get(long id, long? userId)
        {
            var entity = await Uow.Repository<Movie>().GetQueryable().Where(x=>x.Id==id)
                .Include(x=>x.Rates)
                .Include(x=>x.MoviesCountries)
                    .ThenInclude(x=>x.Country)
                .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                .Include(x=>x.MoviePersonsMovies)
                    .ThenInclude(x=>x.Person)
                        .ThenInclude(x=>x.MoviePersonsMovies)
                            .ThenInclude(x=>x.MoviePersonType)
                 .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var genres = entity.Genres.Select(x => x.Genre).Select(x=>x.Title).ToArray();
            var directers = entity.MoviePersonsMovies.Where(x => x.MoviePersonType.Type == "Directer").Select(x =>
                new MoviePersonListViewModel()
                {
                    Id = x.MoviePersonId,
                    Year =x.Person.DateOfBirth.Year,
                    FullName =x.Person.FullName,
                    ImageUrl= x.Person.ImageUrl
                }).ToArray();
            var stars= entity.MoviePersonsMovies.Where(x => x.MoviePersonType.Type == "Star").Select(x =>
                new MoviePersonListViewModel()
                {
                    Id = x.MoviePersonId,
                    Year = x.Person.DateOfBirth.Year,
                    FullName = x.Person.FullName,
                    ImageUrl = x.Person.ImageUrl
                }).ToArray();
            var averageRate = entity.Rates.Any() ? entity.Rates.Sum(x => x.Value) / entity.Rates.Count() : 0;

            var model = new MovieViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Year = entity.Year,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                Genres = genres,
                AverageRate = averageRate,
                Directers = directers,
                UsersRate = (entity.Rates.Any() & userId != null) ? (entity.Rates.FirstOrDefault(rate => rate.ProfileId == userId).Value) : 0,
                Stars = stars,
            };
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
    }
}
