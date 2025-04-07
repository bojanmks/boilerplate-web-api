using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Localization;

namespace WebApi.Implementation.Localization
{
    public class SqlTranslator : ITranslator
    {
        private readonly SqlConnection _connection;
        private readonly IApplicationUser _user;
        private readonly IMemoryCache _memoryCache;

        public SqlTranslator(
            SqlConnection connection,
            IApplicationUser user,
            IMemoryCache memoryCache
        )
        {
            _connection = connection;
            _user = user;
            _memoryCache = memoryCache;
        }

        public string Translate(string key)
        {
            var cachedTranslations = LoadLocalizationData();

            if (cachedTranslations.TryGetValue(key, out string? translatedValue))
            {
                return translatedValue;
            }

            return key;
        }

        private string CacheKey => $"LocalizationCache_{_user.Locale}";

        private Dictionary<string, string> LoadLocalizationData()
        {
            Dictionary<string, string> localizationData;

            if (_memoryCache.TryGetValue(CacheKey, out localizationData))
            {
                return localizationData;
            }

            localizationData = new Dictionary<string, string>();

            var query = $"SELECT [Key], [Value] FROM Translations WHERE Locale = '{_user.Locale}'";
            var command = new SqlCommand(query, _connection);

            _connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                localizationData.Add(reader.GetString(0), reader.GetString(1));
            }

            _connection.Close();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(12));

            _memoryCache.Set(CacheKey, localizationData, cacheEntryOptions);

            return localizationData;
        }
    }
}
