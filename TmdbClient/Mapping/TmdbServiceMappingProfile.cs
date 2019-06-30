using AutoMapper;
using MyIMDB.Data.Entities;
using TmdbClient.ApiModels;

namespace TmdbClient.Mapping
{
    public class TmdbServiceMappingProfile : Profile
    {
        public TmdbServiceMappingProfile()
        {
            CreateMap<TmdbMovie, Movie>().ConvertUsing(new TmdbMovieToMovieConverter());
            CreateMap<Person, MoviePerson>().ConvertUsing(new TmdbPersonToMoviePersonConverter());
        }
    }
}
