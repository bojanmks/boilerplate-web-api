using WebApi.Application.Logging.LoggerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Logging
{
    public interface IUseCaseLogger
    {
        void Log(UseCaseLoggerData data);
    }
}
