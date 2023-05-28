using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Exceptions.Abstraction
{
    public abstract class FormattedException : Exception
    {
        public FormattedException(string message, params object[] values) : base(String.Format(message, values))
        {
        }
    }
}
