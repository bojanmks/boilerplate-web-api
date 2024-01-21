using System.Reflection;

namespace WebApi.Implementation.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static IEnumerable<object> GetAttributesOfType<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            return propertyInfo.GetCustomAttributes(true).Where(x => x is TAttribute);
        }

        public static bool HasAttributeOfType<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            var attributes = propertyInfo.GetAttributesOfType<TAttribute>();

            return attributes.Any();
        }
    }
}
