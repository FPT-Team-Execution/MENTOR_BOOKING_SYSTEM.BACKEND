using System.Linq.Expressions;
using MBS.Core.Common.Pagination;
using Microsoft.EntityFrameworkCore.Query;

namespace MBS.DataAccess.DAO.Interfaces;

public interface IBaseDAO<T> where T : class
{
    Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<Pagination<T>> GetPagingListAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int page = 1,
        int size = 10);

    Task InsertAsync(T entity);
    Task InsertRangeAsync(IEnumerable<T> entities);
    void UpdateAsync(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void DeleteAsync(T entity);
    void DeleteRangeAsync(IEnumerable<T> entities);
}