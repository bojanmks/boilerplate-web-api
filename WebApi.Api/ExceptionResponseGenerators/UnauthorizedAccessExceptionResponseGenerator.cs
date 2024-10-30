using WebApi.Application.ExceptionHandling;

namespace WebApi.Api.ExceptionResponseGenerators
{
    public class UnauthorizedAccessExceptionResponseGenerator : BaseExceptionResponseGenerator<UnauthorizedAccessException>
    {
        protected override ExceptionResponse GenerateAfterCast(UnauthorizedAccessException ex)
        {
            return new ExceptionResponse
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                ShouldBeLogged = false
            };
        }
    }
}
