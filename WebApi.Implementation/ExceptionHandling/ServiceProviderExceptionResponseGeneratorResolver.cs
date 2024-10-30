using WebApi.Application.ExceptionHandling;

namespace WebApi.Implementation.ExceptionHandling
{
    public class ServiceProviderExceptionResponseGeneratorResolver : IExceptionResponseGeneratorResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderExceptionResponseGeneratorResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IExceptionResponseGenerator Resolve(Exception ex)
        {
            var generatorType = GetResponseGeneratorType(ex);
            var generator = _serviceProvider.GetService(generatorType);

            return (IExceptionResponseGenerator)generator;
        }

        private Type GetResponseGeneratorType(Exception ex)
        {
            var exceptionType = ex.GetType();
            var exceptionResponseGeneratorType = typeof(IExceptionResponseGenerator<>);
            var genericType = exceptionResponseGeneratorType.MakeGenericType(exceptionType);

            return genericType;
        }
    }
}
