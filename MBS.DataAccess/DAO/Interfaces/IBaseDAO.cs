using System.Linq.Expressions;
using MBS.Core.Common.Pagination;
using Microsoft.EntityFrameworkCore.Query;

namespace MBS.DataAccess.DAO.Interfaces;

public interface IBaseDAO<T> where T : class
{ 
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

    Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<Pagination<T>> GetPagingListAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int page = 1,
        int size = 10);

    Task<int> InsertAsync(T entity);
    Task<int> InsertRangeAsync(IEnumerable<T> entities);
    int Update(T entity);
    int UpdateRange(IEnumerable<T> entities);
    int Delete(T entity);
    int DeleteRange(IEnumerable<T> entities);
}