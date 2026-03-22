using VaccineManager.Domain.Entities;

namespace VaccineManager.Domain.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    IQueryable<T> GetQueryable();
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    void Delete(T entity);
}