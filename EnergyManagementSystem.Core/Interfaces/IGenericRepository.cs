using System.Linq.Expressions;


namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Temel CRUD operasyonları
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        // Filtreleme için
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}