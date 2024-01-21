using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Localization
{
    public interface ILocaleGetter
    {
        public CultureInfo GetLocale();
    }
}
