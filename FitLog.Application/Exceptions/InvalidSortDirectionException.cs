using FitLog.Application.Exceptions.Abstraction;
using FitLog.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Exceptions
{
    public class InvalidSortDirectionException : TranslatableException
    {
        public InvalidSortDirectionException(ITranslator translator) : base(translator, "invalidSortDirection")
        {
        }
    }
}
