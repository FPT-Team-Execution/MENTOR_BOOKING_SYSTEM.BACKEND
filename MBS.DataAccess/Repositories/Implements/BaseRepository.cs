using MBS.Core.Common.Pagination;
using MBS.DataAccess.DAO.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class BaseRepository<T> : Interfaces.IBaseRepository<T> where T: class
{
    protected readonly IBaseDAO<T> _dao;

    public BaseRepository(IBaseDAO<T> dao)
    {
        _dao = dao;
    }

    public async Task<T?> GetByIdAsync<TKey>(TKey id, string keyName) where TKey : notnull
    {
        return await _dao.SingleOrDefaultAsync(e => EF.Property<TKey>(e, keyName).Equals(id));
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dao.GetListAsync();
    }

    public async Task<Pagination<T>> GetPagedListAsync(int page, int size)
    {
        return await _dao.GetPagingListAsync(
            page: page, 
            size: size);

    }

    public async Task<bool> CreateAsync(T entity)
    {
        return await _dao.InsertAsync(entity) > 0;
    }

    public async Task<bool> CreateRangeAsync(IEnumerable<T> entities)
    {
        return await _dao.InsertRangeAsync(entities) > 0;
    }

    public bool Update(T entity)
    {
        return _dao.Update(entity) > 0;
    }
    public bool UpdateRange(IEnumerable<T> entities)
    {
        return _dao.UpdateRange(entities) > 0;
    }

    public bool Delete(T entity)
    {
        return _dao.Delete(entity) > 0;
    }
    public bool DeleteRange(IEnumerable<T> entities)
    {
        return _dao.DeleteRange(entities) > 0;
    }
}