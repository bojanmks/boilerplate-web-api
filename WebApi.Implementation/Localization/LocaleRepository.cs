namespace WebApi.Implementation.Localization
{
    public static class LocaleRepository
    {
        public static string DefaultLocale => "en-US";
        public static List<string> SupportedLocales => new List<string> { "en-US", "sr-Latn-RS" };
    }
}
