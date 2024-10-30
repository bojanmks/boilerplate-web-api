namespace WebApi.Application.ExceptionHandling
{
    public interface IExceptionResponseGenerator
    {
        ExceptionResponse Generate(Exception ex);
    }

    public interface IExceptionResponseGenerator<TException> : IExceptionResponseGenerator where TException : Exception
    {

    }
}
