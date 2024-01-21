using WebApi.Application.Search.Enums;

namespace WebApi.Application.Search.Attributes
{
    public abstract class BaseSearchAttribute : Attribute
    {
        private readonly ComparisonType _comparisonType;

        public BaseSearchAttribute(ComparisonType comparisonType)
        {
            _comparisonType = comparisonType;
        }

        public ComparisonType ComparisonType => _comparisonType;
    }
}
