using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using System.Collections.Generic;

namespace MyIMDB.Services.MapperProfiles
{
    public class MoviePersonToMoviePersonModelConverter : ITypeConverter<MoviePerson, MoviePersonViewModel>
    {
        public MoviePersonViewModel Convert(MoviePerson source, MoviePersonViewModel destination, ResolutionContext context)
        {
            destination = new MoviePersonViewModel()
            {
                Id = source.Id,
                FullName = source.FullName,
                DateOfBirth = source.DateOfBirth.ToLongDateString(),
                ImageUrl = source.ImageUrl,
                Biography = source.Biography,
                Gender = source.Gender.Title,
                Country = source.Country.Name,
                Movies = context.Mapper.Map<IEnumerable<MoviePersonsMovies>, MovieListViewModel[]>(source.MoviePersonsMovies)
            };

            return destination;
        }
    }
}
