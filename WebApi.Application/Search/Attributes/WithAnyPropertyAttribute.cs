using WebApi.Application.Search.Enums;

namespace WebApi.Application.Search.Attributes
{
    public class WithAnyPropertyAttribute : BaseSearchAttribute
    {
        private readonly string _collection;
        private readonly string _property;
        public WithAnyPropertyAttribute(ComparisonType comparisonType, string collection, string property)
            : base(comparisonType)
        {
            _collection = collection;
            _property = property;
        }

        public string Collection => _collection;
        public string Property => _property;
    }
}
