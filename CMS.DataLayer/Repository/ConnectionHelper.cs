using CMS.DataLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CMS.DataLayer.Repository
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
            // Access the value directly using the indexer
            return bool.Parse(_configuration["DatabaseSettings:UseJsonDb"]);
        }

        public string GetJsonFilePath()
        {
            // Access the value directly using the indexer
            return _configuration["DatabaseSettings:JsonFilePath"];
        }

        public string GetSqlConnectionString()
        {
            // Access the connection string using the indexer
            return _configuration.GetConnectionString("DefaultConnection");
        }
    }
}
