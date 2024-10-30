using WebApi.Application.ExceptionHandling;
using WebApi.Implementation.Exceptions;

namespace WebApi.Api.ExceptionResponseGenerators
{
    public class ForbiddenUseCaseExceptionResponseGenerator : BaseExceptionResponseGenerator<ForbiddenUseCaseException>
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
