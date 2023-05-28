using FitLog.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.DataAccess.Configurations
{
    public class UserConfiguration : SoftDeletableEntityConfiguration<User>
    {
        protected override void ConfigureSoftDeletableEntity(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
