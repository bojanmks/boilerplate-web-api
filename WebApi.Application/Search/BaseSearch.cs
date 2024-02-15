using System.Linq.Expressions;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.Application.Search
{
    public abstract class BaseSearch<TEntity> : ISearchObject<TEntity> where TEntity : Entity
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
        public bool Paginate { get; set; } = false;
        public string SortBy { get; set; } = string.Empty;

        #region Sorting
        private Dictionary<string, Expression<Func<TEntity, object>>> _sortByPropertiesMap = new();

        protected void AddSortByProperty(string propertyName, Expression<Func<TEntity, object>> expression)
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

        protected void AddFilterProperty(Func<object> propertyGetter, Func<object, Expression<Func<TEntity, bool>>> expressionGetter, bool allowNull = false)
        {
            _filterProperties.Add(new SearchFilterPropertyData<TEntity>
            {
                PropertyGetter = propertyGetter,
                ExpressionGetter = expressionGetter,
                AllowNull = allowNull
            });
        }

        public List<Expression<Func<TEntity, bool>>> GetFilterExpressions()
        {
            var expressions = new List<Expression<Func<TEntity, bool>>>();

            foreach (var filterPropertyData in _filterProperties)
            {
                var propertyValue = filterPropertyData.PropertyGetter.Invoke();

                if (propertyValue is not null || filterPropertyData.AllowNull)
                {
                    expressions.Add(filterPropertyData.ExpressionGetter.Invoke(propertyValue));
                }
            }

            return expressions;
        }
        #endregion
    }
}
