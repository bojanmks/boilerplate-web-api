using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.DataAccess.Helpers
{
    public static class LambdaHelper
    {
        public static LambdaExpression ConvertFilterExpression<TInterface>(Expression<Func<TInterface, bool>> filterExpression, Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

            return Expression.Lambda(newBody, newParam);
        }
    }
}
