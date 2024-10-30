using WebApi.Application.ExceptionHandling;
using WebApi.Application.Exceptions;

namespace WebApi.Api.ExceptionResponseGenerators
{
    public class InvalidSortFormatExceptionResponseGenerator : BaseExceptionResponseGenerator<InvalidSortFormatException>
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
