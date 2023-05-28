using FitLog.Application.Logging.LoggerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Logging
{
    public interface IUseCaseLogger
    {
        void Log(UseCaseLoggerData data);
    }
}
