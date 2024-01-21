using WebApi.Application.Logging.LoggerData;

namespace WebApi.Application.Logging
{
    public interface IUseCaseLogger
    {
        void Log(UseCaseLoggerData data);
    }
}
