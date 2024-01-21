namespace WebApi.Implementation
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
