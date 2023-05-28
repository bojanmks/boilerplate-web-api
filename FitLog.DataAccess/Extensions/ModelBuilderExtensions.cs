using FitLog.DataAccess.Entities.Abstraction;
using FitLog.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.DataAccess.Extensions
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
