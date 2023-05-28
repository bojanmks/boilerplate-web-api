using FitLog.Api.ExceptionHandling.Abstraction;
using FitLog.Implementation.Exceptions;

namespace FitLog.Api.ExceptionHandling.ResponseGenerators
{
    public class ForbiddenUseCaseExceptionResponseGenerator : ExceptionResponseGenerator<ForbiddenUseCaseException>
    {
        protected override ExceptionResponse GenerateAfterCast(ForbiddenUseCaseException ex)
        {
            return new ExceptionResponse
            {
                StatusCode = StatusCodes.Status403Forbidden,
                ShouldBeLogged = false
            };
        }
    }
}
