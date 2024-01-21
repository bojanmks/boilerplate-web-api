using WebApi.Application.Extensions;
using WebApi.Application.Search.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Search.Attributes
{
    public class JoinedPropertiesAttribute : BaseSearchAttribute
    {
        private readonly JoinType _joinType;
        private readonly IEnumerable<string> _properties;

        public JoinedPropertiesAttribute(ComparisonType comparisonType, JoinType joinType, params string[] properties) : base(comparisonType)
        {
            _joinType = joinType;
            _properties = properties;
        }

        public JoinType JoinType => _joinType;
        public IEnumerable<string> Properties => _properties;

        public string BuildPropertyConcatanation()
        {
            string separator = _joinType.GetSeparator();

            var propertiesPreparedForExpression = _properties.Select(propertyName => $"x.{propertyName}");

            var concatanationString = string.Join(separator, propertiesPreparedForExpression);

            return $"({concatanationString})";
        }
    }
}
