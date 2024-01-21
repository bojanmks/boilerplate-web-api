using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.DataAccess.Configurations
{
    public abstract class SoftDeletableEntityConfiguration<TEntity> : EntityConfiguration<TEntity>
        where TEntity : SoftDeletableEntity
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            ConfigureSoftDeletableEntity(builder);
        }

        protected abstract void ConfigureSoftDeletableEntity(EntityTypeBuilder<TEntity> builder);
    }
}
