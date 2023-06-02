using FitLog.Application.Search.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Extensions
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
