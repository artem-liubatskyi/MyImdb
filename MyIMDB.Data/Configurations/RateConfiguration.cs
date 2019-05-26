using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.ToTable("Rate");

            builder.HasKey(x => new { x.MovieId, x.ProfileId });

            builder.HasOne(x => x.Movie)
                .WithMany(x=>x.Rates)
                .HasForeignKey(x=>x.MovieId);
        }
    }
}
