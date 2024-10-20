using CMS.Models.DbModel;
using CMS.Repositories.Interfaces;
using CMS.Services;
using Moq;
using Xunit;

namespace CMS.UnitTests.ServiceTests
{
    public class ContactServiceTests
    {
        private readonly Mock<IGenericRepository<Contact>> _contactRepositoryMock;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            _contactRepositoryMock = new Mock<IGenericRepository<Contact>>();
            _contactService = new ContactService(_contactRepositoryMock.Object, true);
        }

        [Fact]
        public async Task GetContactsAsync_ReturnsApiResult()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
                new Contact { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com" }
            };

            _contactRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contacts.AsQueryable());

            // Act
            var result = await _contactService.GetContactsAsync(0, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task GetContactByIdAsync_ReturnsContact()
        {
            // Arrange
            var contact = new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" };
            _contactRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(contact);

            // Act
            var result = await _contactService.GetContactByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task CreateContactAsync_AddsContactWithAutoIncrementedId()
        {
            // Arrange
            var newContact = new Contact { FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" };
            var existingContacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" }
            };
            _contactRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(existingContacts.AsQueryable());

            // Act
            var result = await _contactService.CreateContactAsync(newContact);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id); // Check for auto-incremented ID
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public async Task UpdateContactAsync_UpdatesExistingContact()
        {
            // Arrange
            var updatedContact = new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            _contactRepositoryMock.Setup(repo => repo.UpdateAsync(1, updatedContact)).ReturnsAsync(true);

            // Act
            var result = await _contactService.UpdateContactAsync(1, updatedContact);

            // Assert
            Assert.True(result);
            _contactRepositoryMock.Verify(repo => repo.UpdateAsync(1, updatedContact), Times.Once);
        }

        [Fact]
        public async Task DeleteContactAsync_DeletesExistingContact()
        {
            // Arrange
            _contactRepositoryMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _contactService.DeleteContactAsync(1);

            // Assert
            Assert.True(result);
            _contactRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
