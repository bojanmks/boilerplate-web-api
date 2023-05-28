using FitLog.Api.ExceptionHandling.Abstraction;
using FitLog.Common.DTO;
using System.Reflection;

namespace FitLog.Api.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<TypeData> GetGenericInterfaceImplementationTypes(this Type interfaceType, Assembly[] assemblies)
        {
            var returnData = new List<TypeData>();

            foreach (var assembly in assemblies)
            {
                var allTypesInThisAssembly = assembly.GetTypes();

                foreach (Type implementationType in allTypesInThisAssembly.Where(x => x.IsClass && !x.IsAbstract))
                {
                    var implementedInterface = implementationType.GetInterface(interfaceType.Name.ToString());
                    if (implementedInterface is not null)
                    {
                        returnData.Add(new TypeData
                        {
                            ImplementedInterface = implementedInterface,
                            ImplementationType = implementationType
                        });
                    }
                }
            }

            return returnData;
        }
    }
}
