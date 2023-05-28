using FitLog.Application.Exceptions.Abstraction;
using FitLog.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Exceptions
{
    public class TranslatableUnauthorizedAccessException : TranslatableException
    {
        public TranslatableUnauthorizedAccessException(ITranslator translator, string messageKey) : base(translator, messageKey)
        {
        }
    }
}
