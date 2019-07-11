using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class ReviewConfiguration : BaseConfiguration<Review>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Review");

            builder.HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Movie)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.MovieId);
        }
    }
}
