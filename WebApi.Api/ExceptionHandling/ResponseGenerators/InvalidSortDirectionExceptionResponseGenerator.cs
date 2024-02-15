using WebApi.Api.ExceptionHandling.Abstraction;
using WebApi.Application.Exceptions;

namespace WebApi.Api.ExceptionHandling.ResponseGenerators
{
    public class InvalidSortDirectionExceptionResponseGenerator : ExceptionResponseGenerator<InvalidSortDirectionException>
    {
        protected override ExceptionResponse GenerateAfterCast(InvalidSortDirectionException ex)
        {
            return new ExceptionResponse
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Response = new
                {
                    message = ex.Message
                },
                ShouldBeLogged = false
            };
        }
    }
}
