using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.Interfaces;

namespace MyIMDB.Services
{
    public class MoviePersonService : IMoviePersonService
    {
        private readonly IUnitOfWork Uow;

        public MoviePersonService(IUnitOfWork uow)
        {
            Uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }
        public async Task<MoviePersonViewModel> Get(long id)
        {
            var entity = await Uow.Repository<MoviePerson>().Get(id)
                .Include(x=>x.Country)
                .Include(x=>x.Gender)
                .Include(x=>x.MoviePersonsMovies)
                    .ThenInclude(x=>x.Movie)
                .FirstOrDefaultAsync();
            var movies = entity.MoviePersonsMovies.Select(x => 
            new MovieListViewModel()
            {
                Id= x.MovieId,
                Title=x.Movie.Title,
                Year =x.Movie.Year,
                ImageUrl = x.Movie.ImageUrl,
                AverageRate = x.Movie.AverageRate,
                UsersRate = 0
            }).ToArray();

            var model = new MoviePersonViewModel() {
                Id = entity.Id,
                FullName = entity.FullName,
                DateOfBirth = entity.DateOfBirth.ToLongDateString(),
                ImageUrl = entity.ImageUrl,
                Biography = entity.Biography,
                Gender = entity.Gender.Title,
                Country = entity.Country.Name,
                Movies = movies,
            };

            return model;
        }

        public async Task<IEnumerable<MoviePersonListViewModel>> GetListBySearchQuery(string searchQuerue)
        {
            return await Uow.Repository<MoviePerson>().GetQueryable()
               .Where(x => x.FullName.Contains(searchQuerue))
               .Select(x => new MoviePersonListViewModel() { Id = x.Id, FullName = x.FullName, Year = x.DateOfBirth.Year,ImageUrl = x.ImageUrl })
               .ToArrayAsync();
        }
    }
}
