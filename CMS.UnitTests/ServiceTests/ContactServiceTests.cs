using CMS.Models.DTO;
using CMS.Repositories.Interfaces;
using CMS.Services;
using Moq;
using Xunit;

namespace CMS.UnitTests.ServiceTests
{
    public class ContactServiceTests
    {
        private readonly Mock<IGenericRepository<ContactDTO>> _contactRepositoryMock;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            _contactRepositoryMock = new Mock<IGenericRepository<ContactDTO>>();
            _contactService = new ContactService(_contactRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateContactAsync_ShouldReturnCreatedContactWithId()
        {
            // Arrange
            var contactDto = new ContactDTO { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var existingContacts = new List<ContactDTO>
            {
                new ContactDTO { Id = 1, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            };

            _contactRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(existingContacts.AsQueryable());
            _contactRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ContactDTO>())).Returns(Task.CompletedTask);

            // Act
            var result = await _contactService.CreateContactAsync(contactDto);

            // Assert
            Assert.Equal(2, result.Id); // The new contact should have an Id of 2
            _contactRepositoryMock.Verify(repo => repo.AddAsync(contactDto), Times.Once);
        }

        [Fact]
        public async Task GetContactByIdAsync_ShouldReturnContact()
        {
            // Arrange
            var contactId = 1;
            var contact = new ContactDTO { Id = contactId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            _contactRepositoryMock.Setup(repo => repo.GetByIdAsync(contactId)).ReturnsAsync(contact);

            // Act
            var result = await _contactService.GetContactByIdAsync(contactId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contactId, result.Id);
        }

        [Fact]
        public async Task GetContactsAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            var contacts = new List<ContactDTO>
            {
                new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new ContactDTO { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            };

            _contactRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contacts.AsQueryable());

            // Act
            var result = await _contactService.GetContactsAsync(0, 10);

            // Assert
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(2, result.TotalCount);
        }
    }
}
