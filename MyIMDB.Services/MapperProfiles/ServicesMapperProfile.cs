using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;

namespace MyIMDB.Services.Mappers
{
    public class ServicesMapperProfile : Profile
    {
        public ServicesMapperProfile()
        {
            CreateMap<Movie, MovieListViewModel>();
            CreateMap<Movie, MovieViewModel>();
            CreateMap<MoviePersonsMovies, MoviePersonListViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.MoviePersonId))
                .ForMember(x => x.Year, opt => opt.MapFrom(source => source.Person.DateOfBirth.Year))
                .ForMember(x => x.FullName, opt => opt.MapFrom(source => source.Person.FullName))
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(source => source.Person.ImageUrl));
           
        }
    }
}