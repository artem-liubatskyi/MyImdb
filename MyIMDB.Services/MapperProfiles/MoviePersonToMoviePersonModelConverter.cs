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
                ImageUrl = source.ImageUrl,
                Biography = source.Biography,
                Movies = context.Mapper.Map<IEnumerable<MoviePersonsMovies>, MovieListViewModel[]>(source.MoviePersonsMovies)
            };
            if (source.Country != null)
                destination.Country = source.Country.Name;
            if (source.Gender != null)
                destination.Gender = source.Gender.Title;
            if (source.DateOfBirth != null)
                destination.DateOfBirth = source.DateOfBirth.ToLongDateString();
            return destination;
        }
    }
}
