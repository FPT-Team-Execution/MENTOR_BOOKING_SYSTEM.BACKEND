using MBS.Core.Common.Pagination;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IBaseRepository<T> where T: class
{
    Task<T?> GetByIdAsync<TKey>(TKey id, string keyName) where TKey: notnull;
    Task<IEnumerable<T>> GetAllAsync();
    Task<Pagination<T>> GetPagedListAsync(int page, int size);
    Task<bool> CreateAsync(T entity);
    Task<bool> CreateRangeAsync(IEnumerable<T> entities);
    bool Update(T entity);
    bool Delete(T entity);
}