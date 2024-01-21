using WebApi.Application.Search.Enums;

namespace WebApi.Application.Search.Attributes
{
    public class QueryPropertyAttribute : BaseSearchAttribute
    {
        private readonly IEnumerable<string> _properties;

        public QueryPropertyAttribute(ComparisonType comparisonType, params string[] properties) : base(comparisonType)
        {
            _properties = properties;
        }

        public IEnumerable<string> Properties => _properties;
    }

    public class QueryPropertyAndAttribute : QueryPropertyAttribute, IAndAttribute
    {
        public QueryPropertyAndAttribute(ComparisonType comparisonType, params string[] properties) : base(comparisonType, properties)
        {
        }
    }
}
