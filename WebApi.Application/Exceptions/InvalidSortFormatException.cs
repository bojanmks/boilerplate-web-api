using WebApi.Application.Exceptions.Abstraction;
using WebApi.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Exceptions
{
    public class InvalidSortFormatException : TranslatableException
    {
        public InvalidSortFormatException(ITranslator translator) : base(translator, "invalidSortStringFormat") // Invalid sort string format. (PropertyName1.Direction,PropertyName2.Direction,...)
        {
        }
    }
}
