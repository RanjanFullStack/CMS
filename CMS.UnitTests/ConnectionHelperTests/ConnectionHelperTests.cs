using CMS.DataLayer.Repository;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void UseJsonDatabase_ShouldReturnTrue_WhenConfigured()
        {
            // Arrange
            _configurationMock.Setup(config => config["DatabaseSettings:UseJsonDb"]).Returns("true");

            // Act
            var result = _connectionHelper.UseJsonDatabase();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetJsonFilePath_ShouldReturnConfiguredPath()
        {
            // Arrange
            var expectedPath = "path/to/jsonfile.json";
            _configurationMock.Setup(config => config["DatabaseSettings:JsonFilePath"]).Returns(expectedPath);

            // Act
            var result = _connectionHelper.GetJsonFilePath();

            // Assert
            Assert.Equal(expectedPath, result);
        }

        [Fact]
        public void GetSqlConnectionString_ShouldReturnConfiguredConnectionString()
        {
            // Arrange
            string expectedConnectionString = string.Empty;
            _configurationMock.Setup(config => config.GetConnectionString("DefaultConnection")).Returns(expectedConnectionString);
            // Assuming you want an integer value
            //int myValue = _configuration.GetValue<int>("DefaultConnection"); // Use the correct type here

            // Act
            var result = _connectionHelper.GetSqlConnectionString();

            // Assert
            Assert.Equal(expectedConnectionString, result);
        }
    }
}
