using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;

namespace MyIMDB.Services.MapperProfiles
{
    public class MoviePersonMovieToMovieListConverter : ITypeConverter<MoviePersonsMovies, MovieListViewModel>
    {
        public MovieListViewModel Convert(MoviePersonsMovies source, MovieListViewModel destination, ResolutionContext context)
        {
            destination = new MovieListViewModel()
            {
                Id = source.MovieId,
                Title = source.Movie.Title,
                Year = source.Movie.Year,
                ImageUrl = source.Movie.ImageUrl,
            };
            return destination;
        }
    }
}
