using System.Threading.Tasks;
using CMS.Models.DTO;
using CMS.Models.Helper;

namespace CMS.Services.Interfaces
{
    public interface IContactService
    {
        Task<ApiResult<ContactDTO>> GetContactsAsync(int pageIndex, int pageSize, string sortColumn = null, string sortOrder = null, string filterColumn = null, string filterQuery = null);
        Task<ContactDTO> GetContactByIdAsync(int id);
        Task<ContactDTO> CreateContactAsync(ContactDTO contactDto);
        Task<bool> UpdateContactAsync(int id, ContactDTO contactDto);
        Task<bool> DeleteContactAsync(int id);
    }
}
