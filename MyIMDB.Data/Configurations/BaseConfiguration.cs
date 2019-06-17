using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyIMDB.Data.Abstraction;

namespace MyIMDB.Data.Configurations
{
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(entity => entity.Id);
            ConfigureSpecific(builder);
        }

        public abstract void ConfigureSpecific(EntityTypeBuilder<TEntity> builder);
    }
}
