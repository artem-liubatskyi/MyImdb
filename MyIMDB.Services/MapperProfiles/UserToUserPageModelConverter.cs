using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MyIMDB.ApiModels.Models;
using MyIMDB.Data.Entities;

namespace MyIMDB.Services.MapperProfiles
{
    public class UserToUserPageModelConverter : ITypeConverter<User, UserPageViewModel>
    {
        public UserPageViewModel Convert(User source, UserPageViewModel destination, ResolutionContext context)
        {
            destination = new UserPageViewModel
            {
                FullName = source.FullName
            };
            var movies = context.Mapper.Map<IEnumerable<UserMovie>, IEnumerable<MovieListViewModel>>(source.Movies).ToList();
            destination.Rates = movies.Where(x => x.UsersRate != 0);
            destination.WatchLaterMovies = movies.Where(x => x.isInWatchlist);
            return destination;
        }
    }
}
