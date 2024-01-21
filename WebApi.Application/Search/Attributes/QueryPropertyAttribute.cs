using WebApi.Application.Search.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
