using Microsoft.Extensions.DependencyInjection;
using MyIMDB.DataAccess.Interfaces;

namespace MyIMDB.DataAccess.Configuration
{
    public static class DataAccessDependencies
    {
        public static void RegisterDataAccessDependencies(this IServiceCollection collection)
        {
            collection.AddScoped<IUnitOfWork, UnitOfWork>();
            collection.AddScoped<IUserMovieRepository, UserMovieRepository>();
        }
    }

   
}
