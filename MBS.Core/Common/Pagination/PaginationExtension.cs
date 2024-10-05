namespace MBS.Core.Common.Pagination;

public class PaginationExtension<T> where T : class
{
    
    // Pagination method, either direct from list or via repository
    public static Pagination<T> Paginate(IEnumerable<T> source, int page, int size, int firstPage = 1)
    {
        var pagination = new Pagination<T>();
        if (source == null) throw new ArgumentNullException(nameof(source));
        
        if (size <= 0) throw new ArgumentException("Page size must be greater than 0.", nameof(size));
        
        if (page < firstPage) throw new ArgumentException($"Page number ({page}) must be greater than or equal to the first page ({firstPage}).");
        
        var totalItemCount = source is IQueryable<T> queryable ? queryable.Count() : source.Count();
        
        pagination.PageIndex = page;
        pagination.PageSize = size;
        pagination.TotalPages = (int)Math.Ceiling(totalItemCount / (double) pagination.PageSize);

        if (page > pagination.TotalPages && totalItemCount > 0)
        {
            throw new ArgumentException($"Page number ({page}) exceeds the total pages ({pagination.TotalPages}).");
        }

        if (source is IQueryable<T> query)
        {
            pagination.Items = query.Skip((page - firstPage) * size).Take(size).ToList();
        }
        else
        {
            pagination.Items = source.Skip((page - firstPage) * size).Take(size).ToList();
        }

        return pagination;
    }

}