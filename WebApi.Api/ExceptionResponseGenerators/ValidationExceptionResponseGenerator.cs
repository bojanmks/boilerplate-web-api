using FluentValidation;
using WebApi.Application.ExceptionHandling;

namespace WebApi.Api.ExceptionResponseGenerators
{
    public class ValidationExceptionResponseGenerator : BaseExceptionResponseGenerator<ValidationException>
    {
        private readonly IExceptionResponseGeneratorResolver _responseGeneratorResolver;

        public ValidationExceptionResponseGenerator(IExceptionResponseGeneratorResolver responseGeneratorResolver)
        {
            _responseGeneratorResolver = responseGeneratorResolver;
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
            var customException = ex.Errors.Select(x => x.CustomState).OfType<Exception>().FirstOrDefault();

            if (customException is null)
            {
                return null;
            }

            var responseGenerator = _responseGeneratorResolver.Resolve(customException);

            if (responseGenerator is null)
            {
                return null;
            }

            return responseGenerator.Generate(customException);
        }
    }
}
