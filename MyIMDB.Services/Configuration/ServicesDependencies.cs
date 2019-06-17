using Microsoft.Extensions.DependencyInjection;
using MyIMDB.DataAccess.Configuration;
using MyIMDB.Services.Hashing;

namespace MyIMDB.Services.Configuration
{
    public static class ServicesDependencies
    {
        public static void RegisterServiceDependencies(this IServiceCollection collection)
        {
            collection.AddTransient<IAccountService, AccountService>();
            collection.AddTransient<IHasher, Hasher>();
            collection.AddTransient<IMovieService, MovieService>();
            collection.AddTransient<IMoviePersonService, MoviePersonService>();
            collection.RegisterDataAccessDependencies();
        }
    }
}
