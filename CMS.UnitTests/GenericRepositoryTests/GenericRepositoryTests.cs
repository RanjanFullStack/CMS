using CMS.DataLayer.Interfaces;
using CMS.Models.DbModel;
using CMS.Repositories.Repository;
using Moq;
using Xunit;

namespace CMS.UnitTests.GenericRepositoryTests
{
    public class GenericRepositoryTests
    {
        private readonly Mock<IJsonDbContext> _jsonDbContextMock;
        private readonly Mock<ISqlDbContext> _sqlDbContextMock; // Assuming this is a mockable DbContext
        private readonly GenericRepository<Contact> _repository;
        private readonly List<Contact> _contacts;

        public GenericRepositoryTests()
        {
            _jsonDbContextMock = new Mock<IJsonDbContext>();
            _sqlDbContextMock = new Mock<ISqlDbContext>();
            _contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" },
                new Contact { Id = 2, FirstName = "Bob", LastName = "Jones", Email = "bob@example.com" },
            };

            // Setting up mock for JSON Database
            _jsonDbContextMock.Setup(db => db.GetContacts()).Returns(_contacts);
            _jsonDbContextMock.Setup(db => db.AddContact(It.IsAny<Contact>()))
                .Callback<Contact>(c => _contacts.Add(c)); // Add contact to in-memory list

            // Initialize repository to use JSON database
            _repository = new GenericRepository<Contact>(_jsonDbContextMock.Object, _sqlDbContextMock.Object, true);
        }

        [Fact]
        public async Task GetAllAsync_UsesJsonDb_ReturnsAllContacts()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_UsesJsonDb_ReturnsContact()
        {
            // Arrange
            var expectedContact = new Contact
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com"
            };

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Equal(expectedContact.Id, result.Id);
            Assert.Equal(expectedContact.FirstName, result.FirstName);
            Assert.Equal(expectedContact.LastName, result.LastName);
            Assert.Equal(expectedContact.Email, result.Email);
        }

        [Fact]
        public async Task AddAsync_UsesJsonDb_AddsContact()
        {
            // Arrange
            var newContact = new Contact { FirstName = "Charlie", LastName = "Brown", Email = "charlie.brown@example.com" };

            // Act
            await _repository.AddAsync(newContact);

            // Assert
            _jsonDbContextMock.Verify(db => db.AddContact(It.IsAny<Contact>()), Times.Once);
            Assert.Equal(3, _contacts.Count); // Ensure the contact was added to the in-memory list
            Assert.Contains(newContact, _contacts); // Ensure the new contact is in the list
        }

        [Fact]
        public async Task UpdateAsync_UsesJsonDb_UpdatesContact()
        {
            // Arrange
            //var existingContact = new Contact { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" };
            //_contacts.Add(existingContact);

            var updatedContact = new Contact { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com" };

            // Act
            var result = await _repository.UpdateAsync(1, updatedContact);

            // Assert
            Assert.True(result);
            var updatedContactFromDb = await _repository.GetByIdAsync(1); // Fetch updated contact
            Assert.Equal("Johnson", updatedContactFromDb.LastName); // Check if the last name was updated
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
            var remainingContacts = await _repository.GetAllAsync(); // Fetch remaining contacts after deletion
            Assert.Empty(remainingContacts); // Ensure the contact was removed from the in-memory list
        }

    }
}
