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
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasIndex(e => e.Id);

            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
