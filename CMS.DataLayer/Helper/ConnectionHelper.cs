using CMS.DataLayer.Interfaces;
using Microsoft.Extensions.Options;
using System;

namespace CMS.DataLayer.Helper
{
    public class ConnectionHelper : IConnectionHelper
    {
        private readonly DatabaseSettings _settings;

        public ConnectionHelper(IOptions<DatabaseSettings> options)
        {
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public bool UseJsonDatabase()
        {
            return _settings.UseJsonDb;
        }

        public string GetJsonFilePath()
        {
            if (string.IsNullOrEmpty(_settings.JsonFilePath))
            {
                throw new InvalidOperationException("JsonFilePath is not configured.");
            }
            return _settings.JsonFilePath;
        }

        public string GetSqlConnectionString()
        {
            if (string.IsNullOrEmpty(_settings.DefaultConnection))
            {
                throw new InvalidOperationException("DefaultConnection is not configured.");
            }
            return _settings.DefaultConnection;
        }
    }
}
