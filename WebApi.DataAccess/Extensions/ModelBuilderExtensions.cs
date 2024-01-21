using Microsoft.EntityFrameworkCore;
using WebApi.DataAccess.Entities.Abstraction;
using WebApi.DataAccess.Helpers;

namespace WebApi.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SetupInitialData(this ModelBuilder builder)
        {

        }

        public static void AddQueryFilterForSoftDeletableEntities(this ModelBuilder builder)
        {
            builder.Model.GetEntityTypes()
                         .Where(entityType => typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                         .ToList()
                         .ForEach(entityType =>
                         {
                             builder.Entity(entityType.ClrType)
                                    .HasQueryFilter(LambdaHelper.ConvertFilterExpression<ISoftDeletable>(e => e.IsActive.Value, entityType.ClrType));
                         });
        }
    }
}
