namespace WebApi.Application.ExceptionHandling
{
    public interface IExceptionResponseGeneratorResolver
    {
        public IExceptionResponseGenerator Resolve(Exception ex);
    }
}
