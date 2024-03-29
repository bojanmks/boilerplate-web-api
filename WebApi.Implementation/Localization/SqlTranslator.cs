﻿using Microsoft.Data.SqlClient;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Localization;

namespace WebApi.Implementation.Localization
{
    public class SqlTranslator : ITranslator
    {
        private readonly SqlConnection _connection;
        private readonly IApplicationUser _user;

        private Dictionary<string, string> _localizationData;

        public SqlTranslator(SqlConnection connection, IApplicationUser user)
        {
            _connection = connection;
            _user = user;

            LoadLocalizationData();
        }

        private void LoadLocalizationData()
        {
            _localizationData = new Dictionary<string, string>();

            var query = $"SELECT [Key], [Value] FROM Translations WHERE Locale = '{_user.Locale}'";
            var command = new SqlCommand(query, _connection);

            _connection.Open();

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                _localizationData.Add(reader.GetString(0), reader.GetString(1));
            }

            _connection.Close();
        }

        public string Translate(string key)
        {
            if (_localizationData.TryGetValue(key, out string? translatedValue))
            {
                return translatedValue;
            }

            return key;
        }
    }
}
