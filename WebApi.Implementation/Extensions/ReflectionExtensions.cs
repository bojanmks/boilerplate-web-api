using System.Reflection;

namespace WebApi.Implementation.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetImplementationsOfGenericType(this Assembly assembly, Type genericInterfaceType)
        {
            if (!genericInterfaceType.IsGenericType)
                throw new ArgumentException("Provided type must be a generic type.", nameof(genericInterfaceType));

            return assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType));
        }
    }
}
