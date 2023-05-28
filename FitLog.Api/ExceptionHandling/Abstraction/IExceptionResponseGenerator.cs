namespace FitLog.Api.ExceptionHandling.Abstraction
{
    public interface IExceptionResponseGenerator
    {
        ExceptionResponse Generate(Exception ex);
    }

    public interface IExceptionResponseGenerator<TException> : IExceptionResponseGenerator where TException : Exception
    {

    }
}
