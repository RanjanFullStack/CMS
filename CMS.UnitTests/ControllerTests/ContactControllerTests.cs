using CMS.Controllers;
using CMS.Models.DTO;
using CMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CMS.UnitTests.ControllerTests
{
    public class ContactControllerTests
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly ContactController _contactController;

        public ContactControllerTests()
        {
            _contactServiceMock = new Mock<IContactService>();
            _contactController = new ContactController(_contactServiceMock.Object);
        }

        [Fact]
        public async Task GetContactById_ShouldReturnOkResult_WhenContactExists()
        {
            // Arrange
            var contactId = 1;
            var contact = new ContactDTO { Id = contactId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            _contactServiceMock.Setup(service => service.GetContactByIdAsync(contactId)).ReturnsAsync(contact);

            // Act
            var result = await _contactController.GetContactById(contactId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedContact = Assert.IsType<ContactDTO>(okResult.Value);
            Assert.Equal(contactId, returnedContact.Id);
        }

        [Fact]
        public async Task CreateContact_ShouldReturnCreatedAtActionResult_WhenContactIsCreated()
        {
            // Arrange
            var contactDto = new ContactDTO { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Id = 1 };

            _contactServiceMock.Setup(service => service.CreateContactAsync(contactDto)).ReturnsAsync(contactDto);

            // Act
            var result = await _contactController.CreateContact(contactDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetContactById", createdResult.ActionName);
            Assert.Equal(contactDto, createdResult.Value);
        }

        [Fact]
        public async Task UpdateContact_ShouldReturnNoContent_WhenContactIsUpdated()
        {
            // Arrange
            var contactId = 1;
            var contactDto = new ContactDTO { Id = contactId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            _contactServiceMock.Setup(service => service.UpdateContactAsync(contactId, contactDto)).ReturnsAsync(true);

            // Act
            var result = await _contactController.UpdateContact(contactId, contactDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteContact_ShouldReturnNoContent_WhenContactIsDeleted()
        {
            // Arrange
            var contactId = 1;

            _contactServiceMock.Setup(service => service.DeleteContactAsync(contactId)).ReturnsAsync(true);

            // Act
            var result = await _contactController.DeleteContact(contactId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
