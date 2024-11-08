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

    public Task<Group> GetGroupByProjectAndStudentIdAsync(Guid projectId, string studentId)
    {
        
            return _dao.SingleOrDefaultAsync(x => x.StudentId == studentId && x.ProjectId == projectId);
        
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

    //public async Task<bool> RemoveGroupByProjectAndStudentIdAsync(Guid projectId, string studentId )
    //{
    //    var check = false;
    //    try
    //    {
    //        var groupFound = await _dao.SingleOrDefaultAsync(m => m.ProjectId == projectId && m.StudentId == studentId);
    //        _dao.Delete(groupFound);
    //        check = true;
    //    }
    //    catch (Exception)
    //    {

    //        check = false;
    //    }
    //    return check;
    //}
}