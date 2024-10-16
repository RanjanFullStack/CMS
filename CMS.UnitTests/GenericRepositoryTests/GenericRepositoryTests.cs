using CMS.DataLayer.Repository;
using CMS.Models.DatabaseContext;
using CMS.Models.DTO;
using CMS.Repositories.Repository;
using Moq;
using Xunit;

namespace CMS.UnitTests.GenericRepositoryTests
{
    public class GenericRepositoryTests
    {
        private readonly Mock<JsonDbContext> _jsonDbContext;
        private readonly GenericRepository<ContactDTO> _repository;
        private readonly List<ContactDTO> _contacts;

        public GenericRepositoryTests()
        {
            _jsonDbContext = new Mock<JsonDbContext>();
            _contacts = new List<ContactDTO>();
            _repository = new GenericRepository<ContactDTO>(_jsonDbContext.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddContact()
        {
            // Arrange
            var contact = new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            // Act
            await _repository.AddAsync(contact);
            var allContacts = await _repository.GetAllAsync();

            // Assert
            Assert.Single(allContacts);
            Assert.Equal(contact, allContacts.First());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectContact()
        {
            // Arrange
            var contact = new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            await _repository.AddAsync(contact);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contact.Id, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllContacts()
        {
            // Arrange
            var contact1 = new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var contact2 = new ContactDTO { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" };
            await _repository.AddAsync(contact1);
            await _repository.AddAsync(contact2);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}
