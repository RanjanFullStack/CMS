using CMS.Models.DatabaseContext;
using CMS.Models.DbModel;
using CMS.Repositories.Interfaces;
using CMS.Repositories.Repository;
using UnitOfWork.Interfaces;

namespace UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JsonDbContext _context;
        private IGenericRepository<Contact> _contacts;

        public UnitOfWork(JsonDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Contact> Contacts => _contacts ??= new GenericRepository<Contact>(_context);

        public async Task<int> CompleteAsync()
        {
            await _context.SaveChangesAsync(); // Simulated for JSON context
            return 1; // Returning 1 as a placeholder for actual save changes logic
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
