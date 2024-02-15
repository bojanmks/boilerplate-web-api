using System.Linq.Expressions;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public interface ISearchObject<TEntity> where TEntity : Entity
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public bool Paginate { get; set; }
        public string SortBy { get; set; }

        Expression<Func<TEntity, object>> GetSortByPropertyExpression(string propertyName);
        List<Expression<Func<TEntity, bool>>> GetFilterExpressions();
    }
}
