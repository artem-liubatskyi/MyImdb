using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Entities;

namespace MyIMDB.Data.Configurations
{
    public class GenderConfiguration : BaseConfiguration<Gender>
    {
        public override void ConfigureSpecific(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable("Gender");
        }
    }
}
