using CMS.Models.DbModel;
using CMS.Repositories.Interfaces;
using Moq;
using UnitOfWork.Interfaces;
using Xunit;

namespace CMS.UnitTests.UnitOfWorkTests
{
    public class UnitOfWorkTests
    {
        private readonly Mock<IGenericRepository<Contact>> _mockContactRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UnitOfWorkTests()
        {
            _mockContactRepo = new Mock<IGenericRepository<Contact>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public void Contacts_ReturnsContactRepository()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Contacts).Returns(_mockContactRepo.Object);

            // Act
            var result = _mockUnitOfWork.Object.Contacts;

            // Assert
            Assert.Equal(_mockContactRepo.Object, result);
        }

        [Fact]
        public async Task CompleteAsync_CallsMethodOnce()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _mockUnitOfWork.Object.CompleteAsync();

            // Assert
            Assert.Equal(1, result);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public void Dispose_CallsDispose()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Dispose());

            // Act
            _mockUnitOfWork.Object.Dispose();

            // Assert
            _mockUnitOfWork.Verify(u => u.Dispose(), Times.Once);
        }
    }
}
