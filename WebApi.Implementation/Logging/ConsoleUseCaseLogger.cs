using WebApi.Application.Logging;
using WebApi.Application.Logging.LoggerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
