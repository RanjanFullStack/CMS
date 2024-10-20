using CMS.Models.DbModel;
using CMS.Models.Helper;
using CMS.Models.RequestModel;

namespace CMS.Services.Interfaces
{
    public interface IContactService
    {
        Task<ApiResult<Contact>> GetContactsAsync(int pageIndex, int pageSize, string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null);
        Task<Contact> GetContactByIdAsync(int id);
        Task<Contact> CreateContactAsync(ContactRequest contactRequest);
        Task<bool> UpdateContactAsync(int id, Contact contact);
        Task<bool> DeleteContactAsync(int id);
    }
}
