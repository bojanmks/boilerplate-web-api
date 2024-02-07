using System.Reflection;

namespace WebApi.Api.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetImplementationsOfType<T>(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
        }
    }
}
