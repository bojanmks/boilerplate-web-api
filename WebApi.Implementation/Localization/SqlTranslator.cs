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

        private Dictionary<string, string> LoadLocalizationData()
        {
            string cacheKey = MakeCacheKey();

            Dictionary<string, string> localizationData;

            if (_memoryCache.TryGetValue(cacheKey, out localizationData))
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

            _memoryCache.Set(cacheKey, localizationData, cacheEntryOptions);

            return localizationData;
        }

        private string MakeCacheKey()
        {
            return $"LocalizationCache_{_user.Locale}";
        }
    }
}
