using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.DataAccess.Entities;

namespace WebApi.DataAccess.Configurations
{
    public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
    {
        public void Configure(EntityTypeBuilder<Translation> builder)
        {
            builder.Property(x => x.Key).IsRequired();
            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.Locale).IsRequired();

            builder.HasIndex(x => x.Key);
            builder.HasIndex(x => x.Value);

            builder.HasIndex(x => new { x.Key, x.Locale }).IsUnique();
        }
    }
}
