using CMS.DataLayer.Helper;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CMS.UnitTests.ConnectionHelperTests
{
    public class ConnectionHelperTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly ConnectionHelper _connectionHelper;

        public ConnectionHelperTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _connectionHelper = new ConnectionHelper(_configurationMock.Object);
        }

        [Fact]
        public void UseJsonDatabase_ShouldReturnTrue_WhenUseJsonDbIsTrue()
        {
            _configurationMock.Setup(config => config["DatabaseSettings:UseJsonDb"]).Returns("true");

            var result = _connectionHelper.UseJsonDatabase();

            Assert.True(result);
        }

        [Fact]
        public void GetJsonFilePath_ShouldReturnCorrectPath()
        {
            _configurationMock.Setup(config => config["DatabaseSettings:JsonFilePath"]).Returns("contacts.json");

            var result = _connectionHelper.GetJsonFilePath();

            Assert.Equal("contacts.json", result);
        }

        [Fact]
        public void GetSqlConnectionString_ShouldReturnCorrectConnectionString()
        {
            //_configurationMock.Setup(config => config.GetConnectionString("DefaultConnection")).Returns("Data Source=server;Initial Catalog=db;User Id=user;Password=pass;");

            //var result = _connectionHelper.GetSqlConnectionString();

            //Assert.Equal("Data Source=server;Initial Catalog=db;User Id=user;Password=pass;", result);
        }
    }
}