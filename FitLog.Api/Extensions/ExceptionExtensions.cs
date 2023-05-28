using FitLog.Api.ExceptionHandling.Abstraction;
using System.Reflection;

namespace FitLog.Api.Extensions
{
    public static class ExceptionExtensions
    {
        public static Type GetResponseGeneratorType(this Exception ex)
        {
            var exceptionType = ex.GetType();
            var exceptionResponseGeneratorType = typeof(IExceptionResponseGenerator<>);
            var genericType = exceptionResponseGeneratorType.MakeGenericType(exceptionType);

            return genericType;
        }
    }
}
