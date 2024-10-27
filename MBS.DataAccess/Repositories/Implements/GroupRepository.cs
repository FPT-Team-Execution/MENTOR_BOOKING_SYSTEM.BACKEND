using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class GroupRepository(IBaseDAO<Group> dao) : BaseRepository<Group>(dao), IGroupRepository
{
    public Task<Group> GetGroupByIdAsync(Guid id)
    {
        return _dao.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Group>> GetGroupByProjectIdAsync(Guid project)
    {
        return await _dao.GetListAsync(a => a.ProjectId == project);
    }
    public async Task<Pagination<Group>> GetGroupsByStudentId(string studentId, int page, int size, string sortOrder)
    {
        return await _dao.GetPagingListAsync(
            predicate: g => g.StudentId == studentId,
            include: p => p.Include(x => x.Project),
            orderBy: p => (sortOrder.ToLower() == "asc") ? p.OrderBy(x => x.Project.CreatedOn) : p.OrderByDescending(x => x.Project.CreatedOn),
            page: page,
            size: size
        );
    }

    public async Task<Pagination<Group>> GetPagedListBaseAsync(int page, int size)
    {
        return await _dao.GetPagingListAsync
            (
                include: p => p.Include(
                    x => x.Project).Include(y => y.Student).Include(z => z.Position)
                );
    }



}