using CMS.Models.DTO;
using CMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, [FromQuery] string sortColumn = null, [FromQuery] string sortOrder = null, [FromQuery] string filterColumn = null, [FromQuery] string filterQuery = null)
        {
            var result = await _contactService.GetContactsAsync(pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactDTO contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdContact = await _contactService.CreateContactAsync(contactDto);
            return CreatedAtAction(nameof(GetContactById), new { id = createdContact.Id }, createdContact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDTO contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _contactService.UpdateContactAsync(id, contactDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var deleted = await _contactService.DeleteContactAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
