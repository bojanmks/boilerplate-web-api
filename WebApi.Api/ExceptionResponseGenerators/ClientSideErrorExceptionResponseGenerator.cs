using WebApi.Application.ExceptionHandling;
using WebApi.Implementation.Exceptions;

namespace WebApi.Api.ExceptionResponseGenerators
{
    public class ClientSideErrorExceptionResponseGenerator : BaseExceptionResponseGenerator<ClientSideErrorException>
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
