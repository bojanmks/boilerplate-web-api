using WebApi.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Exceptions.Abstraction
{
    public abstract class TranslatableFormattedException : FormattedException
    {
        public TranslatableFormattedException(ITranslator translator, string messageKey, params object[] values) : base(translator.Translate(messageKey), values)
        {
        }
    }
}
