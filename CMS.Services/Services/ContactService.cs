using CMS.Models.DTO;
using CMS.Models.Helper;
using CMS.Repositories.Interfaces;
using CMS.Services.Interfaces;

namespace CMS.Services
{
    public class ContactService : IContactService
    {
        private readonly IGenericRepository<ContactDTO> _contactRepository;

        public ContactService(IGenericRepository<ContactDTO> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<ApiResult<ContactDTO>> GetContactsAsync(int pageIndex, int pageSize, string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null)
        {
            var contacts = await _contactRepository.GetAllAsync();
            return await ApiResult<ContactDTO>.CreateAsync(contacts.AsQueryable(), pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
        }

        public async Task<ContactDTO> GetContactByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(id);
        }

        public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDto)
        {
            // Fetch existing contacts to determine the next ID
            var contacts = await _contactRepository.GetAllAsync();
            contactDto.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1; // Auto-incrementing ID logic
            await _contactRepository.AddAsync(contactDto);
            return contactDto; // Return the created contact with the new ID
        }

        public async Task<bool> UpdateContactAsync(int id, ContactDTO contactDto)
        {
            return await _contactRepository.UpdateAsync(id, contactDto);
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            return await _contactRepository.DeleteAsync(id);
        }
    }
}
