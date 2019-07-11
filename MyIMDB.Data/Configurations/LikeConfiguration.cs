using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Like");

            builder.HasKey(x => new { x.UserId, x.ReviewId});

            builder.HasOne(x => x.User)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Review)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
