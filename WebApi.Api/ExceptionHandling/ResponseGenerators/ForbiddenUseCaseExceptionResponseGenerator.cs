using WebApi.Api.ExceptionHandling.Abstraction;
using WebApi.Implementation.Exceptions;

namespace WebApi.Api.ExceptionHandling.ResponseGenerators
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
