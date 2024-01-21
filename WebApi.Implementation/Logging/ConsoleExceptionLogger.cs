using WebApi.Application.Logging;

namespace WebApi.Implementation.Logging
{
    public class ConsoleExceptionLogger : IExceptionLogger
    {
        public void Log(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
