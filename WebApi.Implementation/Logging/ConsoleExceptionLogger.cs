using WebApi.Application.Logging;

namespace WebApi.Implementation.Logging
{
    public class ConsoleExceptionLogger : IExceptionLogger
    {
        public Task Log(Exception ex)
        {
            Console.WriteLine(ex);
            return Task.CompletedTask;
        }
    }
}
