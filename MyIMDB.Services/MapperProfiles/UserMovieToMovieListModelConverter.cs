using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;

namespace MyIMDB.Services.MapperProfiles
{
    public class UserMovieToMovieListModelConverter : ITypeConverter<UserMovie, MovieListViewModel>
    {
        public MovieListViewModel Convert(UserMovie source, MovieListViewModel destination, ResolutionContext context)
        {
            destination = new MovieListViewModel
            {
                Id = source.MovieId,
                Title = source.Movie.Title,
                Year = source.Movie.Year,
                ImageUrl = source.Movie.ImageUrl,
                isInWatchlist = source.IsInWatchlist,
                AverageRate = source.Movie.RatesCount == 0 ? 0 : source.Movie.RatesSum / source.Movie.RatesCount,
                UsersRate = source.Rate
            };
            return destination;
        }
    }
}
