using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.DataAccess.Entities;

namespace WebApi.DataAccess.Configurations
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
