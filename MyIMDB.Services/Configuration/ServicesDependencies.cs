using Microsoft.Extensions.DependencyInjection;
using MyIMDB.DataAccess.Configuration;
using MyIMDB.Services.Hashing;
using MyIMDB.Services.Helpers;

namespace MyIMDB.Services.Configuration
{
    public static class ServicesDependencies
    {
        public static void RegisterServiceDependencies(this IServiceCollection collection)
        {
            collection.RegisterDataAccessDependencies();
            collection.AddTransient<IAccountService, AccountService>();
            collection.AddTransient<IJwtFactory, JwtFactory>();
            collection.AddTransient<IHasher, Hasher>();
            collection.AddTransient<IMovieService, MovieService>();
            collection.AddTransient<IMoviePersonService, MoviePersonService>();
            collection.AddTransient<ISeedService, SeedService>();
        }
    }
}
