using FitLog.Application.Exceptions.Abstraction;
using FitLog.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Exceptions
{
    public class InvalidSortFormatException : TranslatableException
    {
        public InvalidSortFormatException(ITranslator translator) : base(translator, "invalidSortStringFormat") // Invalid sort string format. (PropertyName1.Direction,PropertyName2.Direction,...)
        {
        }
    }
}
