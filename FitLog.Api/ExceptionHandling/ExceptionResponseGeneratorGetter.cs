using FitLog.Api.ExceptionHandling.Abstraction;
using FitLog.Api.Extensions;

namespace FitLog.Api.ExceptionHandling
{
    public class ExceptionResponseGeneratorGetter : IExceptionResponseGeneratorGetter
    {
        private readonly IServiceProvider _serviceProvider;

        public ExceptionResponseGeneratorGetter(IServiceProvider serviceProvider)
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
