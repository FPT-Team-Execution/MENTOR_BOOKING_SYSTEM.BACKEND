using System.Linq.Expressions;
using MBS.Core.Common;
using MBS.Core.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MBS.DataAccess.DAO.Implements;

public class BaseDAO<T> where T : class	
{
	private readonly MBSContext _context;
	internal DbSet<T> dbSet;

	public BaseDAO(MBSContext context)
	{
		_context = context;
		dbSet = _context.Set<T>();
	}
    public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
	        IQueryable<T> query = dbSet;
	        if (include != null) query = include(query);

	        if (predicate != null) query = query.Where(predicate);

	        if (orderBy != null) return await orderBy(query).FirstOrDefaultAsync();

	        return await query.FirstOrDefaultAsync();
        }
        
        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
	        IQueryable<T> query = dbSet;

	        if (include != null) query = include(query);

	        if (predicate != null) query = query.Where(predicate);

	        if (orderBy != null) return await orderBy(query).AsNoTracking().ToListAsync();

	        return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Pagination<T>> GetPagingListAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int page = 1,
	        int size = 10)
        {
	        IQueryable<T> query = dbSet.AsNoTracking();
	        if(include != null) query = include(query);
	        if(predicate != null) query = query.Where(predicate);
	        if (orderBy != null)  orderBy(query);
            return PaginationExtension<T>.Paginate(query, page, size, 1);
	 
        }
        public async Task InsertAsync(T entity)
        {
            if (entity == null) return;
            await dbSet.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }
        public void UpdateAsync(T entity)
        {
            dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
        }

        public void DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteRangeAsync(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
}