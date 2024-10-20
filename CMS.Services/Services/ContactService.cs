using CMS.Models.DbModel;
using CMS.Models.Helper;
using CMS.Models.RequestModel;
using CMS.Repositories.Interfaces;
using CMS.Services.Interfaces;

namespace CMS.Services
{
    public class ContactService : IContactService
    {
        private readonly IGenericRepository<Contact> _contactRepository;
        private readonly bool _useJsonDb;

        public ContactService(IGenericRepository<Contact> contactRepository, bool useJsonDb)
        {
            _contactRepository = contactRepository;
            _useJsonDb = useJsonDb;
        }

        public async Task<ApiResult<Contact>> GetContactsAsync(int pageIndex, int pageSize, string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null)
        {
            var contacts = await _contactRepository.GetAllAsync();

            if (_useJsonDb) // Handle JSON Db synchronously
            {
                return ApiResult<Contact>.Create(contacts.AsQueryable(), pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
            }
            else
            {
                return await ApiResult<Contact>.CreateAsync(contacts.AsQueryable(), pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
            }
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return await _contactRepository.GetByIdAsync(id);
        }

        public async Task<Contact> CreateContactAsync(ContactRequest contactRequest)
        {
            var contacts = await _contactRepository.GetAllAsync();
            var autoIncrementContactId = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1; // Auto-incrementing ID logic
            var contact = new Contact() 
            { 
                Id = autoIncrementContactId, 
                FirstName = contactRequest.FirstName, 
                LastName = contactRequest.LastName, 
                Email = contactRequest.Email 
            };
            await _contactRepository.AddAsync(contact);
            return contact; // Return the created contact with the new ID
        }

        public async Task<bool> UpdateContactAsync(int id, Contact contactRequest)
        {
            return await _contactRepository.UpdateAsync(id, contactRequest);
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            return await _contactRepository.DeleteAsync(id);
        }
    }
}
