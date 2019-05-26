using System;
using System.Threading.Tasks;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace MyIMDB.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork Uow;

        public MovieService(IUnitOfWork uow)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }
        public async Task<MovieViewModel> Get(long id, long? userId)
        {
            var entity = await Uow.Repository<Movie>().Get(id)
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
                .Include(movie => movie.Rates)
                .Where(x => x.Title.Contains(searchQuerue))
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
        public async Task<IEnumerable<MovieListViewModel>> GetTop(long? userId)
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
                .Take(250)
                //.ProjectTo<MovieListViewModel>()
                .ToArrayAsync();
        }
        public async Task AddRate(RateViewModel model)
        {
            var movie =  await Uow.Repository<Movie>().Get(model.MovieId)
                .Include(x => x.Rates)
                .FirstOrDefaultAsync();
            if(movie!=null)
            {
                var rate = movie.Rates.FirstOrDefault(x => x.ProfileId == model.UserId);
                if (rate == null)
                    movie.Rates.ToList().Add(new Rate() { MovieId = model.MovieId, ProfileId = model.UserId, Value = model.Value });
                else
                    rate.Value = model.Value;
                Uow.SaveChanges();
            }
            
        }
    }
}
