using Microsoft.EntityFrameworkCore;
using MyIMDB.Data.Configurations;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data
{
    public class ImdbContext : DbContext
    {
        public ImdbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MoviesCountriesConfiguration())
                        .ApplyConfiguration(new CountryConfiguration())
                        .ApplyConfiguration(new GenderConfiguration())
                        .ApplyConfiguration(new GenreConfiguration())
                        .ApplyConfiguration(new MovieConfiguration())
                        .ApplyConfiguration(new MoviesGenresConfiguration())
                        .ApplyConfiguration(new MoviePersonConfiguration())
                        .ApplyConfiguration(new MoviePersonsMoviesConfiguration())
                        .ApplyConfiguration(new MoviePersonTypeConfiguration())
                        .ApplyConfiguration(new UserMoviesConfiguration())
                        .ApplyConfiguration(new UserConfiguration())
                        .ApplyConfiguration(new RefrestTokenConfuguration());

            base.OnModelCreating(modelBuilder);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviePerson> MoviePersons { get; set; }

        public DbSet<UserMovie> UserMovies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<MoviePersonType> MoviePersonsType { get; set; }

        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MoviesCountries> MoviesCountries { get; set; }
        public DbSet<MoviePersonsMovies> MoviePersonsMovies { get; set; }

    }
}
