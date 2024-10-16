using System.Linq.Expressions;

namespace CMS.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task<bool> UpdateAsync(int id, T entity);
        Task<bool> DeleteAsync(int id);
    }
}
