using FitLog.DataAccess.Entities.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.DataAccess.Configurations
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
