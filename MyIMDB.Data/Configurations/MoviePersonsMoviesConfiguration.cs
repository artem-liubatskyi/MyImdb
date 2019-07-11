using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class MoviePersonsMoviesConfiguration : IEntityTypeConfiguration<MoviePersonsMovies>
    {
        public void Configure(EntityTypeBuilder<MoviePersonsMovies> builder)
        {
            builder.ToTable("MoviePersonMovie");

            builder.HasKey(x => new { x.MovieId, x.MoviePersonId ,x.MoviePersonTypeId, x.Character});

            builder.HasOne(x => x.Movie)
              .WithMany(x => x.MoviePersonsMovies)
              .HasForeignKey(x => x.MovieId);

            builder.HasOne(x => x.Person)
                .WithMany(x => x.MoviePersonsMovies)
                .HasForeignKey(x => x.MoviePersonId);

            builder.HasOne(x => x.MoviePersonType)
                .WithMany(x => x.MoviePersonsMovies)
                .HasForeignKey(x => x.MoviePersonTypeId);
        }
    }
}
