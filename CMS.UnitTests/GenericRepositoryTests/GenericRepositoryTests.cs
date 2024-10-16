using CMS.DataLayer.Context;
using CMS.Models.DbModel;
using CMS.Repositories.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CMS.UnitTests.GenericRepositoryTests
{
    public class GenericRepositoryTests
    {
        private readonly DbContextOptions<JsonDbContext> _options;
        private readonly JsonDbContext _context;
        private readonly GenericRepository<Contact> _repository;

        public GenericRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<JsonDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new JsonDbContext(_options);
            _repository = new GenericRepository<Contact>(_context);

            // Ensure database is empty before each test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Seed data
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
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var entity = new Contact { Id = 3, FirstName = "Bill", LastName = "Gates", Email = "bill.gates@example.com" };

            // Act
            await _repository.AddAsync(entity);

            // Assert
            var addedEntity = await _context.Contacts.FindAsync(3);
            Assert.NotNull(addedEntity);
            Assert.Equal("Bill", addedEntity.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity_WhenEntityExists()
        {
            // Arrange
            var entity = new Contact { Id = 1, FirstName = "UpdatedFirstName", LastName = "Doe", Email = "john.doe@example.com" };

            // Act
            var result = await _repository.UpdateAsync(1, entity);

            // Assert
            Assert.True(result);
            var updatedEntity = await _context.Contacts.FindAsync(1);
            Assert.Equal("UpdatedFirstName", updatedEntity.FirstName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity_WhenEntityExists()
        {
            // Act
            var result = await _repository.DeleteAsync(1);

            // Assert
            Assert.True(result);
            var deletedEntity = await _context.Contacts.FindAsync(1);
            Assert.Null(deletedEntity);
        }
    }
}
