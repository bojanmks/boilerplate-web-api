using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation.Localization
{
    public static class LocaleRepository
    {
        public static string DefaultLocale => "en-US";
        public static List<string> SupportedLocales => new List<string> { "en-US", "sr-Latn-RS" };
    }
}
