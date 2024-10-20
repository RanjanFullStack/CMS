using CMS.Controllers;
using CMS.Models.DbModel;
using CMS.Models.RequestModel;
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
            var contact = new Contact { Id = contactId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            _contactServiceMock.Setup(service => service.GetContactByIdAsync(contactId)).ReturnsAsync(contact);

            // Act
            var result = await _contactController.GetContactById(contactId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedContact = Assert.IsType<Contact>(okResult.Value);
            Assert.Equal(contactId, returnedContact.Id);
        }

        [Fact]
        public async Task CreateContact_ShouldReturnCreatedAtActionResult_WhenContactIsCreated()
        {
            // Arrange
            var contactRequest = new ContactRequest { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var contact = new Contact { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Id = 1 };

            _contactServiceMock.Setup(service => service.CreateContactAsync(contactRequest)).ReturnsAsync(contact);

            // Act
            var result = await _contactController.CreateContact(contactRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetContactById", createdResult.ActionName);
            Assert.Equal(contact, createdResult.Value);
        }

        [Fact]
        public async Task UpdateContact_ShouldReturnNoContent_WhenContactIsUpdated()
        {
            // Arrange
            var contactId = 1;
            var contactRequest = new Contact { Id = contactId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            _contactServiceMock.Setup(service => service.UpdateContactAsync(contactId, contactRequest)).ReturnsAsync(true);

            // Act
            var result = await _contactController.UpdateContact(contactId, contactRequest);

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
