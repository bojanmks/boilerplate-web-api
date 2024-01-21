using WebApi.Api.ExceptionHandling.Abstraction;
using WebApi.Application.Exceptions;

namespace WebApi.Api.ExceptionHandling.ResponseGenerators
{
    public class EntityNotFoundExceptionResponseGenerator : ExceptionResponseGenerator<EntityNotFoundException>
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
