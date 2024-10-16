using CMS.DataLayer.Helper;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CMS.UnitTests.ConnectionHelperTests
{
    public class ConnectionHelperTests
    {
        private readonly Mock<IOptions<DatabaseSettings>> _mockOptions;
        private readonly ConnectionHelper _connectionHelper;

        public ConnectionHelperTests()
        {
            _mockOptions = new Mock<IOptions<DatabaseSettings>>();
            _mockOptions.Setup(o => o.Value).Returns(new DatabaseSettings
            {
                UseJsonDb = true,
                JsonFilePath = "path/to/jsonfile",
                DefaultConnection = "Data Source=server;Initial Catalog=db;User Id=user;Password=pass;"
            });

            _connectionHelper = new ConnectionHelper(_mockOptions.Object);
        }

        [Fact]
        public void UseJsonDatabase_ShouldReturnTrue_WhenConfigurationIsTrue()
        {
            // Act
            var result = _connectionHelper.UseJsonDatabase();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetJsonFilePath_ShouldReturnFilePath_WhenConfigurationIsValid()
        {
            // Act
            var result = _connectionHelper.GetJsonFilePath();

            // Assert
            Assert.Equal("path/to/jsonfile", result);
        }

        [Fact]
        public void GetSqlConnectionString_ShouldReturnConnectionString_WhenConfigured()
        {
            // Act
            var result = _connectionHelper.GetSqlConnectionString();

            // Assert
            Assert.Equal("Data Source=server;Initial Catalog=db;User Id=user;Password=pass;", result);
        }
    }
}
