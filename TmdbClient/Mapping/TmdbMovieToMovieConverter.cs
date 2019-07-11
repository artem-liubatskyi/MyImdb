using AutoMapper;
using MyIMDB.Data.Entities;
using TmdbClient.ApiModels;

namespace TmdbClient.Mapping
{
    class TmdbMovieToMovieConverter : ITypeConverter<TmdbMovie, Movie>
    {
        public Movie Convert(TmdbMovie source, Movie destination, ResolutionContext context)
        {
            var date = source.Release_date.Split('-');
            destination = new Movie
            {
                Title = source.Title,
                Year = System.Convert.ToInt32(date[0]),
                ImageUrl = source.Poster_path,
                Description = source.Overview,
                Budget = source.Budget,
                Runtime = source.Runtime
            };
            return destination;
        }
    }
}
