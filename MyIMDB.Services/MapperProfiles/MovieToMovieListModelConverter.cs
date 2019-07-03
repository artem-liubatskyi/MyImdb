using System.Linq;
using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;

namespace MyIMDB.Services.MapperProfiles
{
    public class MovieToMovieListModelConverter : ITypeConverter<Movie, MovieListViewModel>
    {
        public MovieListViewModel Convert(Movie source, MovieListViewModel destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            destination = new MovieListViewModel
            {
                Id = source.Id,
                Title = source.Title,
                Year = source.Year,
                ImageUrl = source.ImageUrl,

                AverageRate = source.RatesSum == 0 ? 0 : source.RatesSum / source.RatesCount
            };

            context.Items.TryGetValue("userId", out object userId);

            if (userId == null)
                return destination;

            if (source.UserMovies == null || !source.UserMovies.Any())
                return destination;

            var userRate = source.UserMovies.Where(x => x.UserId == (long)userId).FirstOrDefault();

            if (userRate == null)
            {
                destination.UsersRate = 0;
                destination.isInWatchlist = false;
            }
            else
            {
                destination.UsersRate = userRate.Rate;
                destination.isInWatchlist = userRate.IsInWatchlist;
            }
            
            return destination;
        }
    }
}
