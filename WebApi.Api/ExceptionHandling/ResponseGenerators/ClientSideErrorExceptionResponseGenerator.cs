using WebApi.Api.ExceptionHandling.Abstraction;
using WebApi.Implementation.Exceptions;

namespace WebApi.Api.ExceptionHandling.ResponseGenerators
{
    public class ClientSideErrorExceptionResponseGenerator : ExceptionResponseGenerator<ClientSideErrorException>
    {
        protected override ExceptionResponse GenerateAfterCast(ClientSideErrorException ex)
        {
            return new ExceptionResponse
            {
                StatusCode = StatusCodes.Status409Conflict,
                Response = new
                {
                    message = ex.Message
                },
                ShouldBeLogged = false
            };
        }
    }
}
