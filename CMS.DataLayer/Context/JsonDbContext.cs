using System.Text.Json;
using CMS.DataLayer.Interfaces;
using CMS.Models.DbModel;

namespace CMS.DataLayer.Context
{
    public class JsonDbContext : IJsonDbContext
    {
        private readonly string _jsonFilePath;
        private List<Contact> _contacts;

        public JsonDbContext(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(_jsonFilePath))
            {
                var json = File.ReadAllText(_jsonFilePath);
                _contacts = JsonSerializer.Deserialize<List<Contact>>(json) ?? [];
            }
            else
            {
                _contacts = [];
            }
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_contacts);
            File.WriteAllText(_jsonFilePath, json);
        }

        public virtual List<Contact> GetContacts() => _contacts;

        public void AddContact(Contact contact)
        {
            // Manual ID Auto-Increment
            contact.Id = _contacts.Any() ? _contacts.Max(c => c.Id) + 1 : 1;
            _contacts.Add(contact);
            SaveData();
        }

        public void UpdateContact(Contact contact)
        {
            var existingContact = _contacts.FirstOrDefault(c => c.Id == contact.Id);
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Email = contact.Email;
                SaveData();
            }
        }

        public void DeleteContact(int id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                _contacts.Remove(contact);
                SaveData();
            }
        }
    }
}
