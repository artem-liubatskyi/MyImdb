using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;

namespace MyIMDB.Services.MapperProfiles
{
    public class UserMovieToMovieListModelConverter : ITypeConverter<UserMovie, MovieListViewModel>
    {
        public MovieListViewModel Convert(UserMovie source, MovieListViewModel destination, ResolutionContext context)
        {
            destination = new MovieListViewModel();
            destination.Id = source.MovieId;
            destination.Title = source.Movie.Title;
            destination.Year = source.Movie.Year;
            destination.ImageUrl = source.Movie.ImageUrl;
            destination.isInWatchlist = source.IsInWatchlist;
            destination.AverageRate = source.Movie.RatesCount == 0 ? 0 : source.Movie.RatesSum / source.Movie.RatesCount;
            destination.UsersRate = source.Rate;
            return destination;
        }
    }
}
