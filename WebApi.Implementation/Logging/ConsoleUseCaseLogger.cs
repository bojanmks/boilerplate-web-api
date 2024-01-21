using WebApi.Application.Logging;
using WebApi.Application.Logging.LoggerData;

namespace WebApi.Implementation.Logging
{
    public class ConsoleUseCaseLogger : IUseCaseLogger
    {
        public void Log(UseCaseLoggerData data)
        {
            Console.WriteLine(data);
        }
    }
}
