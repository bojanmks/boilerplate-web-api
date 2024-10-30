namespace WebApi.Application.ExceptionHandling
{
    public class ExceptionResponse
    {
        public int StatusCode { get; set; } = 500;
        public object? Response { get; set; }
        public bool ShouldBeLogged { get; set; } = true;
    }
}
