using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Localization
{
    public interface ITranslator
    {
        string Translate(string key);
    }
}
