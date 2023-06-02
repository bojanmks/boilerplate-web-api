using FitLog.Api.ExceptionHandling.Abstraction;
using FitLog.Application.Exceptions;

namespace FitLog.Api.ExceptionHandling.ResponseGenerators
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
