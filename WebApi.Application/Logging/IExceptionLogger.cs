namespace WebApi.Application.Logging
{
    public interface IExceptionLogger
    {
        void Log(Exception ex);
    }
}
