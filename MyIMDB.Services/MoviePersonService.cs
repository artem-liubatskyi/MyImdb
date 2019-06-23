using MyIMDB.ApiModels.Models;
using MyIMDB.DataAccess.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var entity = await Uow.MoviePersonRepository.GetFull(id);

            var movies = entity.MoviePersonsMovies.Select(x =>
            new MovieListViewModel()
            {
                Id = x.MovieId,
                Title = x.Movie.Title,
                Year = x.Movie.Year,
                ImageUrl = x.Movie.ImageUrl,
                UsersRate = 0
            }).ToArray();

            var model = new MoviePersonViewModel()
            {
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
    }
}
