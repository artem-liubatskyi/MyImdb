using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    class MoviePersonTypeConfiguration : BaseConfiguration<MoviePersonType>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<MoviePersonType> builder)
        {
            builder.ToTable("MoviePersonType");
        }
    }
}
