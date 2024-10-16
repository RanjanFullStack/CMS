using CMS.Models.DbModel;
using CMS.Repositories.Interfaces;

namespace UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Contact> Contacts { get; }
        Task<int> CompleteAsync();
    }

}
