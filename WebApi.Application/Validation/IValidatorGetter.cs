using FluentValidation;

namespace WebApi.Application.Validation
{
    public interface IValidatorGetter
    {
        AbstractValidator<T> GetValidator<T>();
    }
}
