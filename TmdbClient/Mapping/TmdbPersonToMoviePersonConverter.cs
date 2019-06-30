using AutoMapper;
using MyIMDB.Data.Entities;
using System;
using TmdbClient.ApiModels;

namespace TmdbClient.Mapping
{
    public class TmdbPersonToMoviePersonConverter : ITypeConverter<Person, MoviePerson>
    {
        public MoviePerson Convert(Person source, MoviePerson destination, ResolutionContext context)
        {
            destination = new MoviePerson
            {
                FullName = source.Name,
                Biography = source.Biography,
                ImageUrl = source.Profile_path
            };
            if (source.Birthday != null)
                destination.DateOfBirth = DateTime.Parse(source.Birthday);
            return destination;
        }
    }
}
