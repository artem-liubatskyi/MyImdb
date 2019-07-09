using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class RefrestTokenConfuguration : BaseConfiguration<RefreshToken>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("Token");
            //builder.HasOne(x => x.User);
        }
    }
}
