using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Implementation
{
    public static class ServiceProviderGetter
    {
        private static IServiceProvider _provider;

        public static void SetupProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public static IServiceProvider GetProvider() => _provider;
    }
}
