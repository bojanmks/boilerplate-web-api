using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Validation;

namespace WebApi.Implementation.Validators
{
    public class ServiceProviderValidatorGetter : IValidatorGetter
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderValidatorGetter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public AbstractValidator<T> GetValidator<T>()
        {
            return _serviceProvider.GetService<AbstractValidator<T>>();
        }
    }
}
