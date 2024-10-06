using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MBS.Core.Common.Pagination;
using Microsoft.EntityFrameworkCore.Query;

namespace MBS.DataAccess.Repositories.Implements
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly MBSContext _context;
        internal DbSet<T> dbSet;

        public BaseRepository(MBSContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public async Task<bool> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync2(T entity)
        {
            dbSet.Update(entity);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            return false;
        }
        
        #region Upgraded Repository
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
        #endregion
        
        
    }
}
