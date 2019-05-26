using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class MoviesCountriesConfiguration : IEntityTypeConfiguration<MoviesCountries>
    {
        public void Configure(EntityTypeBuilder<MoviesCountries> builder)
        {
            builder.ToTable("MovieCountry");

            builder.HasKey(x => new { x.MovieId, x.CountryId });

            builder.HasOne(x => x.Movie)
                .WithMany(x => x.MoviesCountries)
                .HasForeignKey(x => x.MovieId);

            builder.HasOne(x => x.Country)
                .WithMany(x => x.MoviesCountries)
                .HasForeignKey(x => x.MovieId);
        }
    }
}
