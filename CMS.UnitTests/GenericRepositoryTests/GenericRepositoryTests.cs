using CMS.DataLayer.Context;
using CMS.DataLayer.Interfaces;
using CMS.Models.DbModel;
using Moq;
using Xunit;

namespace CMS.UnitTests.GenericRepositoryTests
{
    public class GenericRepositoryTests
    {
        private readonly Mock<JsonDbContext> _jsonDbContextMock;
        private readonly Mock<SqlDbContext> _sqlDbContextMock; // Assuming this is a mockable DbContext
        private readonly GenericRepository<Contact> _repository;
        private readonly List<Contact> _contacts;

        public GenericRepositoryTests()
        {
            _jsonDbContextMock = new Mock<JsonDbContext>();
            _sqlDbContextMock = new Mock<SqlDbContext>();
            _contacts =
            [
                // Arrange
                new Contact { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" },
                new Contact { Id = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com" },
            ];

            // Setting up mock for JSON Database
            _jsonDbContextMock.Setup(db => db.GetContacts()).Returns(_contacts);

            // Initialize repository to use JSON database
            _repository = new GenericRepository<Contact>(_jsonDbContextMock.Object, _sqlDbContextMock.Object, true);
        }

        [Fact]
        public async Task GetAllAsync_UsesJsonDb_ReturnsAllContacts()
        {
            // Arrange
            _contacts.Add(new Contact { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" });
            _contacts.Add(new Contact { Id = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com" });

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_UsesJsonDb_ReturnsContact()
        {
            // Arrange
            var contact = new Contact { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" };
            _contacts.Add(contact);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Equal(contact, result);
        }

        [Fact]
        public async Task AddAsync_UsesJsonDb_AddsContact()
        {
            // Arrange
            var newContact = new Contact { FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" };

            // Act
            await _repository.AddAsync(newContact);

            // Assert
            _jsonDbContextMock.Verify(db => db.AddContact(It.IsAny<Contact>()), Times.Once);
            Assert.Single(_contacts); // Ensure the contact was added to the in-memory list
        }

        [Fact]
        public async Task UpdateAsync_UsesJsonDb_UpdatesContact()
        {
            // Arrange
            var existingContact = new Contact { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" };
            _contacts.Add(existingContact);

            var updatedContact = new Contact { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com" };

            // Act
            var result = await _repository.UpdateAsync(1, updatedContact);

            // Assert
            Assert.True(result);
            Assert.Equal("Johnson", _contacts.First().LastName); // Check if the last name was updated
        }

        [Fact]
        public async Task DeleteAsync_UsesJsonDb_DeletesContact()
        {
            // Arrange
            var existingContact = new Contact { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" };
            _contacts.Add(existingContact);

            // Act
            var result = await _repository.DeleteAsync(1);

            // Assert
            Assert.True(result);
            Assert.Empty(_contacts); // Ensure the contact was removed from the in-memory list
        }
    }
}
