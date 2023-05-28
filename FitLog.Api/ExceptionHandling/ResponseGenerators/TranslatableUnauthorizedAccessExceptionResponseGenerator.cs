using FitLog.Api.ExceptionHandling.Abstraction;
using FitLog.Implementation.Exceptions;

namespace FitLog.Api.ExceptionHandling.ResponseGenerators
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
