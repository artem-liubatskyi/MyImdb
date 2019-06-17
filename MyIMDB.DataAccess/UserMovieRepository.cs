using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess.Interfaces;

namespace MyIMDB.DataAccess
{
    public class UserMovieRepository : IUserMovieRepository
    {
        protected DbContext DbContext;

        public UserMovieRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<UserMovie> Add(UserMovie movie)
        {
            await DbContext.AddAsync(movie);
            return movie;
        }

        public UserMovie Update(UserMovie entity)
        {
            return DbContext.Update(entity).Entity;
        }
    }
}
