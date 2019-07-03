using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class GenreConfiguration : BaseConfiguration<Genre>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genre");
            builder.HasIndex(x => x.Title).IsUnique();
        }
    }
}
