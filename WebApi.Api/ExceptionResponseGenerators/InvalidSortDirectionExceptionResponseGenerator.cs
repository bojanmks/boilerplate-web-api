using WebApi.Application.ExceptionHandling;
using WebApi.Application.Exceptions;

namespace WebApi.Api.ExceptionResponseGenerators
{
    public class InvalidSortDirectionExceptionResponseGenerator : BaseExceptionResponseGenerator<InvalidSortDirectionException>
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
