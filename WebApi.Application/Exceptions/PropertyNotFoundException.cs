using WebApi.Application.Exceptions.Abstraction;
using WebApi.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Exceptions
{
    public class PropertyNotFoundException : TranslatableFormattedException
    {
        public PropertyNotFoundException(ITranslator translator, string propName) : base(translator, "propertyNotFound", propName)
        {
        }
    }
}
