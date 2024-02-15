using System.Linq.Expressions;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public class EfBaseSearch<TEntity> : BaseSearch where TEntity : Entity
    {
        #region Sorting
        private Dictionary<string, Expression<Func<TEntity, object>>> _sortByPropertiesMap = new();

        protected void DefineSortProperty(string propertyName, Expression<Func<TEntity, object>> expression)
        {
            _sortByPropertiesMap.Add(propertyName.ToLower(), expression);
        }

        public Expression<Func<TEntity, object>> GetSortByPropertyExpression(string propertyName)
        {
            if (_sortByPropertiesMap.TryGetValue(propertyName.ToLower(), out var expression))
            {
                return expression;
            }

            return null;
        }
        #endregion

        #region Filtering
        private List<SearchFilterPropertyData<TEntity>> _filterProperties = new();

        protected void DefineFilterProperty(Func<object> propertyGetter, Func<object, Expression<Func<TEntity, bool>>> expressionGetter, bool allowFilteringByNull = false)
        {
            _filterProperties.Add(new SearchFilterPropertyData<TEntity>
            {
                PropertyGetter = propertyGetter,
                ExpressionGetter = expressionGetter,
                AllowFilteringByNull = allowFilteringByNull
            });
        }

        public List<Expression<Func<TEntity, bool>>> GetFilterExpressions()
        {
            var expressions = new List<Expression<Func<TEntity, bool>>>();

            foreach (var filterPropertyData in _filterProperties)
            {
                var propertyValue = filterPropertyData.PropertyGetter.Invoke();

                if (propertyValue is not null || filterPropertyData.AllowFilteringByNull)
                {
                    expressions.Add(filterPropertyData.ExpressionGetter.Invoke(propertyValue));
                }
            }

            return expressions;
        }
        #endregion
    }
}
