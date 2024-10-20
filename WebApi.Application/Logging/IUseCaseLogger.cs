using WebApi.Application.Logging.LoggerData;

namespace WebApi.Application.Logging
{
    public interface IUseCaseLogger
    {
        Task Log(UseCaseLoggerData data);
    }
}
