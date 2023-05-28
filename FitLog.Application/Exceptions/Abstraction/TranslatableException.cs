using FitLog.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Exceptions.Abstraction
{
    public abstract class TranslatableException : Exception
    {
        public TranslatableException(ITranslator translator, string messageKey) : base(translator.Translate(messageKey))
        {
        }
    }
}
