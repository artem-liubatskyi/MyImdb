using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class CountryConfiguration : BaseConfiguration<Country>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Country");
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
