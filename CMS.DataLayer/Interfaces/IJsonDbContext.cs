using CMS.Models.DbModel;

namespace CMS.DataLayer.Interfaces
{
    public interface IJsonDbContext
    {
        void AddContact(Contact contact);
        void DeleteContact(int id);
        List<Contact> GetContacts();
        void UpdateContact(Contact contact);
    }
}