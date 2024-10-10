using MBS.Core.Common.Pagination;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<Pagination<T>> GetPagedListAsync(int page, int size);
    Task<bool> CreateAsync(T entity);
    bool Update(T entity);
    bool Delete(T entity);


}