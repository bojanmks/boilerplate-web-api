using WebApi.Api.ExceptionHandling.Abstraction;
using WebApi.Api.Extensions;

namespace WebApi.Api.ExceptionHandling
{
    public class ServiceProviderExceptionResponseGeneratorGetter : IExceptionResponseGeneratorGetter
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderExceptionResponseGeneratorGetter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IExceptionResponseGenerator? Get(Exception ex)
        {
            var generatorType = ex.GetResponseGeneratorType();
            var generator = _serviceProvider.GetService(generatorType);

            return (IExceptionResponseGenerator?)generator;
        }
    }
}
