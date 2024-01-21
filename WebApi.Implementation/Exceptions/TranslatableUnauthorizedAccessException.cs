using WebApi.Application.Exceptions.Abstraction;
using WebApi.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Implementation.Exceptions
{
    public class TranslatableUnauthorizedAccessException : TranslatableException
    {
        public TranslatableUnauthorizedAccessException(ITranslator translator, string messageKey) : base(translator, messageKey)
        {
        }
    }
}
