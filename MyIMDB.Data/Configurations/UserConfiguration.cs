using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class UserConfiguration : BaseConfiguration<User>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasOne(x => x.Gender);

            builder.HasOne(x => x.Country);

            builder.HasOne(x => x.Role);

            builder.HasOne(x => x.Token)
                   .WithOne(x => x.User)
                   .HasForeignKey<RefreshToken>(t => t.UserId);
        }
    }
}
