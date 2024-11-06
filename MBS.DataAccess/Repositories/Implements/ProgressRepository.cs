using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class ProgressRepository(IBaseDAO<Progress> dao) : BaseRepository<Progress>(dao), IProgressRepository
{
    public Task<Pagination<Progress>> GetProgressesAsync(Guid projectId, int pageNumber, int pageSize, string sortOrder = "asc")
    {
        var progress = _dao.GetPagingListAsync(
            predicate: p => p.ProjectId == projectId,
            orderBy: q => sortOrder == "asc" ? q.OrderBy(p => p.CreatedOn) : q.OrderByDescending(p => p.CreatedOn),
            include: q => q.Include(p => p.Project),
            page: pageSize,
            size: pageSize
            );
        return null;
    }

    public async Task<Progress?> GetProgressByIdAsync(Guid id)
    {
        return await _dao.SingleOrDefaultAsync(predicate: p => p.Id == id);
    }
}