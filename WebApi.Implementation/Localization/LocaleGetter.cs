﻿using System.Globalization;
using Microsoft.AspNetCore.Http;
using WebApi.Application.Localization;

namespace WebApi.Implementation.Localization
{
    public class LocaleGetter : ILocaleGetter
    {
        private readonly IHttpContextAccessor _accessor;

        public LocaleGetter(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public CultureInfo GetLocale()
        {
            if (_accessor == null)
            {
                return DefaultLocale;
            }

            if (_accessor.HttpContext == null)
            {
                return DefaultLocale;
            }

            var request = _accessor.HttpContext.Request;
            var localeCode = request.Headers["Accept-Language"];

            if (LocaleConstants.SupportedLocales.Contains(localeCode))
            {
                return new CultureInfo(localeCode);
            }

            return DefaultLocale;
        }

        public CultureInfo DefaultLocale => new CultureInfo(LocaleConstants.DefaultLocale);
    }
}
