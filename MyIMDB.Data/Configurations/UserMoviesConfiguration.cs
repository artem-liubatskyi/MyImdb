using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class UserMoviesConfiguration : IEntityTypeConfiguration<UserMovie>
    {
        public void Configure(EntityTypeBuilder<UserMovie> builder)
        {
            builder.ToTable("UserMovie");

            builder.HasKey(x => new { x.MovieId, x.UserId });

            builder.HasOne(x => x.Movie)
                .WithMany(x => x.UserMovies)
                .HasForeignKey(x => x.MovieId);

            builder.HasOne(x => x.User)
               .WithMany(x => x.Movies)
               .HasForeignKey(x => x.UserId);
        }
    }
}
