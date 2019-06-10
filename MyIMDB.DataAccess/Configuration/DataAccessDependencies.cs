using Microsoft.Extensions.DependencyInjection;
using MyIMDB.Interfaces;

namespace MyIMDB.DataAccess.Configuration
{
    public static class DataAccessDependencies
    {
        public static IServiceCollection RegisterDataAccessDependencies(this IServiceCollection collection)
        {
            collection.AddScoped<IUnitOfWork, UnitOfWork>();
            collection.AddTransient<IRateRepository, RateRepository>();
            collection.AddTransient<IWatchlistRepository, WatchlistRepository>();
            return collection;
        }
    }

   
}
