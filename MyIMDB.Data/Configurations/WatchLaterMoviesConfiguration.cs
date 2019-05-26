using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class WatchLaterMoviesConfiguration : IEntityTypeConfiguration<WatchLaterMovies>
    {
        public void Configure(EntityTypeBuilder<WatchLaterMovies> builder)
        {
            builder.ToTable("WatchLaterMovie");

            builder.HasKey(x => new { x.UsereId, x.MovieId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.WatchLaterList)
                .HasForeignKey(x => x.UsereId);
        }
    }
}
