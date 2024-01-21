using WebApi.Api.ExceptionHandling.Abstraction;
using WebApi.Implementation.Exceptions;

namespace WebApi.Api.ExceptionHandling.ResponseGenerators
{
    public class TranslatableUnauthorizedAccessExceptionResponseGenerator : ExceptionResponseGenerator<TranslatableUnauthorizedAccessException>
    {
        protected override ExceptionResponse GenerateAfterCast(TranslatableUnauthorizedAccessException ex)
        {
            return new ExceptionResponse
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Response = new
                {
                    message = ex.Message
                },
                ShouldBeLogged = false
            };
        }
    }
}
