using WebApi.Application.ExceptionHandling;
using WebApi.Application.Exceptions;

namespace WebApi.Api.ExceptionResponseGenerators
{
    public class EntityNotFoundExceptionResponseGenerator : BaseExceptionResponseGenerator<EntityNotFoundException>
    {
        protected override ExceptionResponse GenerateAfterCast(EntityNotFoundException ex)
        {
            return new ExceptionResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                ShouldBeLogged = false
            };
        }
    }
}
