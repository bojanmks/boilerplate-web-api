using WebApi.Application.ExceptionHandling;

namespace WebApi.Api.ExceptionResponseGenerators
{
    public abstract class BaseExceptionResponseGenerator<TException> : IExceptionResponseGenerator<TException> where TException : Exception
    {
        public ExceptionResponse Generate(Exception ex)
        {
            return GenerateAfterCast(ex as TException);
        }

        protected abstract ExceptionResponse GenerateAfterCast(TException ex);
    }
}
