using FitLog.Api.ExceptionHandling.Abstraction;
using FluentValidation;

namespace FitLog.Api.ExceptionHandling.ResponseGenerators
{
    public class ValidationExceptionResponseGenerator : ExceptionResponseGenerator<ValidationException>
    {
        private readonly IExceptionResponseGeneratorGetter _responseGeneratorGetter;

        public ValidationExceptionResponseGenerator(IExceptionResponseGeneratorGetter responseGeneratorGetter)
        {
            _responseGeneratorGetter = responseGeneratorGetter;
        }

        protected override ExceptionResponse GenerateAfterCast(ValidationException ex)
        {
            var customExceptionResponse = GetCustomExceptionResponse(ex);
            if (customExceptionResponse is not null)
            {
                return customExceptionResponse;
            }

            return new ExceptionResponse
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Response = new
                {
                    errors = ex.Errors.Select(x => new
                    {
                        property = x.PropertyName.Substring(x.PropertyName.LastIndexOf('.') + 1),
                        error = x.ErrorMessage
                    })
                },
                ShouldBeLogged = false
            };
        }

        private ExceptionResponse GetCustomExceptionResponse(ValidationException ex)
        {
            var customException = ex.Errors.Select(x => x.CustomState as Exception)
                                           .FirstOrDefault(customState => customState is Exception);

            if (customException is null)
            {
                return null;
            }

            var responseGenerator = _responseGeneratorGetter.Get(customException);

            if (responseGenerator is null)
            {
                return null;
            }

            return responseGenerator.Generate(customException);
        }
    }
}
