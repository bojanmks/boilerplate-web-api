namespace WebApi.Application.Logging
{
    public interface IExceptionLogger
    {
        Task Log(Exception ex);
    }
}
