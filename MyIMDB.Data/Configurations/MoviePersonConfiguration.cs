using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class MoviePersonConfiguration : BaseConfiguration<MoviePerson>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<MoviePerson> builder)
        {
            builder.ToTable("MoviePerson");
            //builder.HasIndex(x => x.FullName).IsUnique();
            builder.HasOne(x => x.Country);
            builder.HasOne(x=>x.Gender);
        }
    }
}
