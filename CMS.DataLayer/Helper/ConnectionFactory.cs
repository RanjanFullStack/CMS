using CMS.DataLayer.Interfaces;

namespace CMS.DataLayer.Helper
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConnectionHelper _connectionHelper;

        public ConnectionFactory(IConnectionHelper connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }

        public bool UseJsonDatabase() => _connectionHelper.UseJsonDatabase();

        public string GetConnectionString() => _connectionHelper.UseJsonDatabase()
            ? _connectionHelper.GetJsonFilePath()
            : _connectionHelper.GetSqlConnectionString();
    }

}
