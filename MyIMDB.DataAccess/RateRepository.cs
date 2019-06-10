using System;
using MyIMDB.Data;
using MyIMDB.Data.Entities;

namespace MyIMDB.DataAccess
{
    public class RateRepository :IRateRepository
    {
        protected ImdbContext DbContext;

        public RateRepository(ImdbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Rate Add(Rate entity)
        {
            return DbContext.Rates.Add(entity).Entity;
        }

        public Rate Update(Rate entity)
        {
            return DbContext.Update(entity).Entity;
        }
    }
}
