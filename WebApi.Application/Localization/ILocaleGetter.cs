using System.Globalization;

namespace WebApi.Application.Localization
{
    public interface ILocaleGetter
    {
        public CultureInfo GetLocale();
    }
}
