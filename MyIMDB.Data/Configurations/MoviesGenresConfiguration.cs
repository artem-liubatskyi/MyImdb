using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class MoviesGenresConfiguration : IEntityTypeConfiguration<MoviesGenres>
    {
        public void Configure(EntityTypeBuilder<MoviesGenres> builder)
        {
            builder.ToTable("MovieGenre");

            builder.HasKey(x => new { x.MovieId, x.GenreId });

            builder.HasOne(x => x.Genre)
                .WithMany(x => x.MoviesGenres)
                .HasForeignKey(x=>x.GenreId);

            builder.HasOne(x => x.Movie)
                .WithMany(x => x.Genres)
                .HasForeignKey(x => x.MovieId);
        }

    }
}
