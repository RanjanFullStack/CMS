using CMS.DataLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CMS.DataLayer.Helper
{
    public class ConnectionHelper : IConnectionHelper
    {
        private readonly IConfiguration _configuration;

        public ConnectionHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool UseJsonDatabase()
        {
            return bool.TryParse(_configuration["DatabaseSettings:UseJsonDb"], out bool useJson) && useJson;
        }

        public string GetJsonFilePath()
        {
            return _configuration["DatabaseSettings:JsonFilePath"];
        }

        public string GetSqlConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }
    }
}
