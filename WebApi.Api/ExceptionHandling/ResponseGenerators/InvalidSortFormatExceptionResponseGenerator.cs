using WebApi.Api.ExceptionHandling.Abstraction;
using WebApi.Application.Exceptions;

namespace WebApi.Api.ExceptionHandling.ResponseGenerators
{
    public class InvalidSortFormatExceptionResponseGenerator : ExceptionResponseGenerator<InvalidSortFormatException>
    {
        protected override ExceptionResponse GenerateAfterCast(InvalidSortFormatException ex)
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
