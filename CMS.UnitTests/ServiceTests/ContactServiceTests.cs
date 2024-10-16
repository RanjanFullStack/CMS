using CMS.DataLayer.Context;
using CMS.Models.DbModel;
using CMS.Models.DTO;
using CMS.Repositories.Interfaces;
using CMS.Services;
using CMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CMS.UnitTests.ServiceTests
{
    public class ContactServiceTests
    {
        private readonly Mock<IGenericRepository<ContactDTO>> _contactRepositoryMock;
        private readonly JsonDbContext _context;
        private readonly IContactService _contactService;

        public ContactServiceTests()
        {
            _context = new JsonDbContext(new DbContextOptionsBuilder<JsonDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options);

            _contactRepositoryMock = new Mock<IGenericRepository<ContactDTO>>();
            _contactService = new ContactService(_contactRepositoryMock.Object);

            // Ensure database is empty before each test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            SeedData();
        }

        private void SeedData()
        {
            _context.Contacts.AddRange(
                new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new Contact { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetContactsAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            var contacts = _context.Contacts.Select(c => new ContactDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            }).AsQueryable();

            _contactRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(contacts);

            // Act
            var result = await _contactService.GetContactsAsync(1, 10, "FirstName", "asc", null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Data.Count());
        }
    }
}
