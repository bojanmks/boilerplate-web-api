using FitLog.Application.Logging;
using FitLog.Application.Logging.LoggerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Logging
{
    public class ConsoleUseCaseLogger : IUseCaseLogger
    {
        public void Log(UseCaseLoggerData data)
        {
            Console.WriteLine(data);
        }
    }
}
