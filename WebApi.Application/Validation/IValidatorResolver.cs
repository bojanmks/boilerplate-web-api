using FluentValidation;

namespace WebApi.Application.Validation
{
    public interface IValidatorResolver
    {
        AbstractValidator<T> Resolve<T>();
    }
}
