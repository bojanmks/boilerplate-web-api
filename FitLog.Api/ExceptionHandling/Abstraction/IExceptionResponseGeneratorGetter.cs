namespace FitLog.Api.ExceptionHandling.Abstraction
{
    public interface IExceptionResponseGeneratorGetter
    {
        public IExceptionResponseGenerator Get(Exception ex);
    }
}
