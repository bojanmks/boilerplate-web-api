namespace WebApi.Api.ExceptionHandling.Abstraction
{
    public abstract class ExceptionResponseGenerator<TException> : IExceptionResponseGenerator<TException> where TException : Exception
    {
        public ExceptionResponse Generate(Exception ex)
        {
            return GenerateAfterCast(ex as TException);
        }

        protected abstract ExceptionResponse GenerateAfterCast(TException ex);
    }
}
