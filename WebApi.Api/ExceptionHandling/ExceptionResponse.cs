namespace WebApi.Api.ExceptionHandling
{
    public class ExceptionResponse
    {
        public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
        public object? Response { get; set; }
        public bool ShouldBeLogged { get; set; } = true;
    }
}
