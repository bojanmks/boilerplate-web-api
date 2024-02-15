using System.Linq.Expressions;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public class SearchFilterPropertyData<TEntity> where TEntity : Entity
    {
        public Func<object> PropertyGetter { get; set; }
        public Func<object, Expression<Func<TEntity, bool>>> ExpressionGetter { get; set; }
        public bool AllowNull { get; set; }
    }
}
