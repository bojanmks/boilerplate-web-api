using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Validation;

namespace WebApi.Implementation.Validators
{
    public class ServiceProviderValidatorResolver : IValidatorResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderValidatorResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public AbstractValidator<T> Resolve<T>()
        {
            return _serviceProvider.GetService<AbstractValidator<T>>();
        }
    }
}
