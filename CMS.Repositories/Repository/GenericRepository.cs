using CMS.Models.DbModel;
using CMS.Models.DbModel;
using CMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.DataLayer.Context
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly JsonDbContext _jsonDbContext;
        private readonly SqlDbContext _sqlDbContext;
        private readonly bool _useJsonDb;

        public GenericRepository(JsonDbContext jsonDbContext, SqlDbContext sqlDbContext, bool useJsonDb)
        {
            _jsonDbContext = jsonDbContext;
            _sqlDbContext = sqlDbContext;
            _useJsonDb = useJsonDb;
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            if (_useJsonDb)
            {
                if (typeof(T) == typeof(Contact))
                {
                    var contacts = _jsonDbContext.GetContacts()
                        .Select(contact => new Contact
                        {
                            Id = contact.Id,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            Email = contact.Email
                        }).AsQueryable();

                    return (IQueryable<T>)contacts;
                }
            }
            else
            {
                if (typeof(T) == typeof(Contact))
                {
                    var contacts = await _sqlDbContext.Set<Contact>()
                        .Select(contact => new Contact
                        {
                            Id = contact.Id,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            Email = contact.Email
                        }).ToListAsync(); // Use ToListAsync to convert to List<T> for async operations

                    return (IQueryable<T>)contacts.AsQueryable();
                }
            }

            return Enumerable.Empty<T>().AsQueryable();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            if (_useJsonDb)
            {
                return (T)(object)_jsonDbContext.GetContacts().FirstOrDefault(c => c.Id == id);
            }
            else
            {
                return await _sqlDbContext.Set<T>().FindAsync(id);
            }
        }

        public async Task AddAsync(T entity)
        {
            if (_useJsonDb)
            {
                if (entity is Contact contact)
                {
                    _jsonDbContext.AddContact(contact);
                }
            }
            else
            {
                await _sqlDbContext.Set<T>().AddAsync(entity);
            }
        }

        public async Task<bool> UpdateAsync(int id, T entity)
        {
            if (_useJsonDb)
            {
                if (entity is Contact contact)
                {
                    var existingContact = _jsonDbContext.GetContacts().FirstOrDefault(c => c.Id == id);
                    if (existingContact != null)
                    {
                        _jsonDbContext.UpdateContact(contact);
                        return true;
                    }
                }
            }
            else
            {
                _sqlDbContext.Set<T>().Update(entity);
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (_useJsonDb)
            {
                var contact = _jsonDbContext.GetContacts().FirstOrDefault(c => c.Id == id);
                if (contact != null)
                {
                    _jsonDbContext.DeleteContact(id);
                    return true;
                }
            }
            else
            {
                var entity = await _sqlDbContext.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _sqlDbContext.Set<T>().Remove(entity);
                    return true;
                }
            }

            return false;
        }
    }
}
