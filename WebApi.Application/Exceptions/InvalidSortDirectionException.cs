using WebApi.Application.Exceptions.Abstraction;
using WebApi.Application.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Exceptions
{
    public class InvalidSortDirectionException : TranslatableException
    {
        public InvalidSortDirectionException(ITranslator translator) : base(translator, "invalidSortDirection")
        {
        }
    }
}
