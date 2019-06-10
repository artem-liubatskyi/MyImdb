using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyIMDB.Data;
using MyIMDB.Data.Entities;

namespace MyIMDB.DataAccess
{
    public class WatchlistRepository : IWatchlistRepository
    {
        protected ImdbContext DbContext;

        public WatchlistRepository(ImdbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<WatchLaterMovies> AddAsync(WatchLaterMovies entity)
        {
            await DbContext.AddAsync(entity);
            return entity;
        }

        public IQueryable<WatchLaterMovies> GetQueryable()
        {
            return DbContext.WatchLaterMovies;
        }

        public void Remove(WatchLaterMovies entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Attach(entity);
            }

            DbContext.Remove(entity);
        }
    }
}
