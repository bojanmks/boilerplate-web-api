using FitLog.Application.Exceptions.Abstraction;
using FitLog.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Exceptions
{
    public class PropertyNotFoundException : TranslatableFormattedException
    {
        public PropertyNotFoundException(ITranslator translator, string propName) : base(translator, "propertyNotFound", propName)
        {
        }
    }
}
