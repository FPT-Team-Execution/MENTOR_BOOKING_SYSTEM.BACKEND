using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class GroupRepository(IBaseDAO<Group> dao) : BaseRepository<Group>(dao), IGroupRepository
{
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

}