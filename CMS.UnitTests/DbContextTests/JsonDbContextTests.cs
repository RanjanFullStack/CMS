using CMS.Models.DbModel;
using Xunit;
using CMS.DataLayer.Context;

namespace CMS.UnitTests.DbContextTests
{
    public class JsonDbContextTests
    {
        [Fact]
        public void AddContact_ShouldAutoIncrementId()
        {
            // Arrange
            var jsonDbContext = new JsonDbContext("contacts.json");
            var contact1 = new Contact { FirstName = "John", LastName = "Doe", Email = "john@example.com" };
            var contact2 = new Contact { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com" };

            // Act
            jsonDbContext.AddContact(contact1);
            jsonDbContext.AddContact(contact2);

            // Assert
            Assert.Equal(5, contact1.Id);
            Assert.Equal(6, contact2.Id);
        }
    }
}