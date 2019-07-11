using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess;
using MyIMDB.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace MyIMDB.Services.MapperProfiles
{
    public class MovieToMovieModelConverter : ITypeConverter<Movie, MovieViewModel>
    {
        public MovieViewModel Convert(Movie source, MovieViewModel destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            destination = new MovieViewModel
            {
                Id = source.Id,

                Title = source.Title,

                Year = source.Year,

                ImageUrl = source.ImageUrl,

                TrailerUrl = source.TrailerUrl,

                Runtime = MappingStringParser.ParseRuntime(source.Runtime),

                Budget = MappingStringParser.ParseBudget(source.Budget),

                Genres = source.Genres.Select(x => x.Genre).Select(x => x.Title).ToArray(),

                Directors = context.Mapper.Map<IEnumerable<MoviePersonsMovies>, IEnumerable<MoviePersonListViewModel>>
                    (source.MoviePersonsMovies.Where(x => x.MoviePersonType.Type == Constants.DirectorType)),

                Stars = context.Mapper.Map<IEnumerable<MoviePersonsMovies>, IEnumerable<MoviePersonListViewModel>>
                    (source.MoviePersonsMovies.Where(x => x.MoviePersonType.Type == Constants.StarType)),

                Reviews = context.Mapper.Map<IEnumerable<Review>, IEnumerable<ReviewApiModel>>(source.Reviews),

                AverageRate = source.RatesSum == 0 ? 0 : source.RatesSum / source.RatesCount,

                Countries = source.MoviesCountries.Select(x => x.Country.Name),

                Description = source.Description

            };


            context.Items.TryGetValue("userId", out object userId);

            if (userId == null)
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
