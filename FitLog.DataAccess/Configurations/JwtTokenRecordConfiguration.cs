using FitLog.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.DataAccess.Configurations
{
    public class JwtTokenRecordConfiguration : EntityConfiguration<JwtTokenRecord>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<JwtTokenRecord> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.JwtTokenRecords)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
