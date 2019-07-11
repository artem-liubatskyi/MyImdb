using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;
using System;

namespace MyIMDB.Services.MapperProfiles
{
    public class ServicesMapperProfile : Profile
    {
        public ServicesMapperProfile()
        {
            CreateMap<MoviePersonsMovies, MoviePersonListViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.MoviePersonId))
                .ForMember(x => x.Year, opt => opt.MapFrom(source => source.Person.DateOfBirth.Year))
                .ForMember(x => x.FullName, opt => opt.MapFrom(source => source.Person.FullName))
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(source => source.Person.ImageUrl))
                .ForMember(x=>x.Character, opt=>opt.MapFrom(source=>source.Character));

            CreateMap<Movie, MovieViewModel>().ConvertUsing(new MovieToMovieModelConverter());

            CreateMap<Movie, MovieListViewModel>().ConvertUsing(new MovieToMovieListModelConverter());

            CreateMap<UserMovie, MovieListViewModel>().ConvertUsing(new UserMovieToMovieListModelConverter());

            CreateMap<User, UserPageViewModel>().ConvertUsing(new UserToUserPageModelConverter());

            CreateMap<MoviePersonsMovies, MovieListViewModel>().ConvertUsing(new MoviePersonMovieToMovieListConverter());

            CreateMap<MoviePerson, MoviePersonViewModel>().ConvertUsing(new MoviePersonToMoviePersonModelConverter());

            CreateMap<RegisterModel, User>();

            CreateMap<Gender, GenderModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(source => source.Title));

            CreateMap<Country, CountryModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(source => source.Name));

            CreateMap<Review, ReviewApiModel>().ConvertUsing(new ReviewToReviewApiConverter());

            CreateMap<ReviewApiModel, Review>()
                .ForMember(x => x.Added, opt => opt.MapFrom(source => DateTime.Now));
        }
    }
}