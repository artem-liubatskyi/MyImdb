using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class MovieConfiguration : BaseConfiguration<Movie>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movie");
            builder.HasIndex(x => new { x.Title, x.Year }).IsUnique();
        }
    }
}
