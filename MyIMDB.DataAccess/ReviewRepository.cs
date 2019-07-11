using Microsoft.EntityFrameworkCore;
using MyIMDB.Data.Entities;
using MyIMDB.DataAccess.Interfaces;
using System;
using System.Threading.Tasks;

namespace MyIMDB.DataAccess
{
    public class ReviewRepository : IReviewRepository
    {
        protected DbContext DbContext;

        public ReviewRepository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Review> AddAsync(Review review)
        {
            await DbContext.AddAsync(review);
            return review;
        }
    }
}
